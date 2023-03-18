using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Creature 
{
    [SerializeField] CreatureBase _base;
    [SerializeField] int level;

    
    public Creature(CreatureBase pbase, int plevel)
    {
        _base = pbase;
        level = plevel;

        Init();
    }
   
    public CreatureBase Base {
        get
        {
            return _base;
        }
    }
   public int Level { get 
        {
            return level;
        } 
    }
    public int Exp { get; set; }
    public int HP { get; set; }

    public int MP { get; set; }
    public List<Move> Moves { get;  set; }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat,int> Stats { get; private set; }

    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Condition Status { get;private set; }

    public int StatusTime { get;  set; }

    public Condition VolitileStatus { get; private set; }

    public int VolitileStatusTime { get; set; }

    public Queue<string> statusChanges { get; private set; }

    

    public event System.Action OnStatusChanged;
    public event System.Action OnHPChanged;
    public void Init()
    {

        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= CreatureBase.maxMoves)
                break;
         }

        Exp = Base.GetExpForLevel(Level);
        CalculateStats();

        HP = MaxHP;
        MP = MaxMP;

        statusChanges = new Queue<string>();
        ResetStatBoost();
        Status = null;
        VolitileStatus = null;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0},
            {Stat.Speed, 0}
        };
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);


        int oldMaxHP = MaxHP;
        MaxHP =  Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10 + Level;

        if(oldMaxHP != 0)
        HP += MaxHP - oldMaxHP;

        int oldmaxmp = MaxMP;
        MaxMP = Mathf.FloorToInt((Base.MaxMP * Level) / 100f) + 10 + Level;

        if (oldmaxmp != 0)
            MP += MaxMP - oldmaxmp;
    }

    public void IncreaseMP(int amount)
    {
        MP = Mathf.Clamp(MaxMP + amount, 0, MP);
    }

    public void SetStatus(ConditionsID conditionsID)
    {
        if (Status != null) return;

        Status = ConditionDB.Conditions[conditionsID];
        Status?.OnStart?.Invoke(this);
        OnStatusChanged?.Invoke();
    }

    public void SetVolitileStatus(ConditionsID conditionsID)
    {
        if (VolitileStatus != null) return;

        VolitileStatus = ConditionDB.Conditions[conditionsID];
        VolitileStatus?.OnStart?.Invoke(this);
    
    }
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }
    public void CureVolitileStatus()
    {
        VolitileStatus = null;
        
    }
    public void DecreaseHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        OnHPChanged?.Invoke();
    }

    public void IncreaseHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        OnHPChanged?.Invoke();
    }
    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];

        int boost = StatBoosts[stat];

        if (boost > 0)
            statusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
        else
            statusChanges.Enqueue($"{Base.Name}'s {stat} fell!");

        var boostvalue = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statValue = Mathf.FloorToInt(statValue * boostvalue[boost]);
            else
            statValue = Mathf.FloorToInt(statValue / boostvalue[-boost]);

        return statValue;
    }

    public void ApplyBoosts(List<StatBoost> statboost)
    {
        foreach (var statBoost in statboost)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = StatBoosts[stat] + boost;

            Debug.Log($"{stat}has been boosted to {statboost}");
        }
    }

    
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

   

    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }

    public int Speed
    {
        get
        {
            return GetStat(Stat.Speed);
        }
    }

    public int MaxHP { get; private set; }

    public int MaxMP { get; private set; }


    public int TakeDamage(Move move,Creature attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.3f)
            critical = 2f;

        
        float type = TypeChart.GetEffectivness(move.Base.Type, this.Base.Type1) * 
            TypeChart.GetEffectivness(move.Base.Type, this.Base.Type2);
      
       

        float modifiers = Random.Range(.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        
        DecreaseHP(damage);

        return damage;
    }

    public Move GetRandomMove()
    {
        var movesWithMP = Moves.Where(x => x.MPCost > 0).ToList();

        int r = Random.Range(0, movesWithMP.Count);
        return movesWithMP[r];
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolitileStatus?.OnAfterTurn?.Invoke(this);
    }

    public bool OnBeforeMove()
    {
        bool canPreformMove = true;
        if(Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
            {
                canPreformMove = false;
            }
            if (VolitileStatus?.OnBeforeMove != null)
            
                if (!VolitileStatus.OnBeforeMove(this))
                {
                    canPreformMove = false;
                }
            }

        return canPreformMove;
    }

    public bool CheckForLevelUp()
    {
        if(Exp > Base.GetExpForLevel(Level + 1))
        {
            ++level;
            CalculateStats();
            return true;
        }

        return false;
    }

    public LearnableMoves GetMoveAtLevel()
    {
       return Base.LearnableMoves.Where(x => x.Level == level).FirstOrDefault();
    }

    public void Heal()
    {
        HP = MaxHP;
        OnHPChanged?.Invoke();

        CureStatus();
    }
    public void LearnMove(MoveBase learnMove)
    {
        if (Moves.Count > CreatureBase.maxMoves)
            return;
        Moves.Add(new Move(learnMove));
    }

    public bool HasMove(MoveBase moveToCheck)
    {
       return Moves.Count(m => m.Base == moveToCheck) > 0;
    }

    public Evolution CheckForEvolution()
    {
        return Base.Evolutions.FirstOrDefault(e => e.RequiredLevel == level);
    }

    public Evolution CheckForEvolution(ItemBase item)
    {
        return Base.Evolutions.FirstOrDefault(e => e.RequiredItem == item);
    }

    public void Evolve(Evolution evolution)
    {
        _base = evolution.EvolveInto;
        CalculateStats();
    }
  
    public void OnBattleOver()
    {
        VolitileStatus = null;
        ResetStatBoost();
    }

    [System.Serializable]
    public class CreatureSaveData
    {
        public string name;
        public int hp;
        public int level;
        public int exp;
        public ConditionsID? statusID;
        public List<MoveSaveData> moves;
    }
    public Creature(CreatureSaveData saveData)
    {
        _base = CreatureDB.GetObjectbyName(saveData.name);
        HP = saveData.hp;
        level = saveData.level;
        Exp = saveData.exp;

        if (saveData.statusID != null)
            Status = ConditionDB.Conditions[saveData.statusID.Value];
        else
            Status = null;

        Moves = saveData.moves.Select(s => new Move(s)).ToList();

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= CreatureBase.maxMoves)
                break;
        }

        CalculateStats();
        statusChanges = new Queue<string>();
        ResetStatBoost();
        VolitileStatus = null;

    }

    
    public CreatureSaveData GetSaveData()
    {
        var savedata = new CreatureSaveData()
        {
            name = Base.name,
            hp = HP,
            level = Level,
            exp = Exp,
            statusID = Status?.ID,
            moves = Moves.Select(m => m.GetSaveData()).ToList()
        };
        return savedata;
    }

}

