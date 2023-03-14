using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moves", menuName = "Creature/Create Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] CreatureType type;
    [SerializeField] int power;
    [SerializeField] int mpCost;
    [SerializeField] int accuracy;
    [SerializeField] int speed;
    [SerializeField] bool alwaysHits;
    [SerializeField] bool isSpecial;
    [SerializeField] MoveCatagory catagory;
    [SerializeField] MoveEffects effects;
    [SerializeField] public AttackEffect attackVisualEffect;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;
    public string Name
    {
        get { return name; }
    }

    public string Description
    {

        get { return description; }
    }

    public CreatureType Type
    {
        get { return type; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public int MPCost
    {
        get { return mpCost; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public bool AlwaysHits
    {
        get { return alwaysHits; }
    }

    public MoveCatagory Catagory
    {
        get { return catagory; }
    }
    public MoveEffects Effects
    {
        get { return effects; }
    }

    public List<SecondaryEffects> Secondaries
    {
        get { return secondaries; }
    }
    public MoveTarget Target
    {
        get { return target; }
    }
}

    [System.Serializable]
    public class MoveEffects
    {
        [SerializeField] List<StatBoost> boosts;
        [SerializeField] ConditionsID status;
        [SerializeField] ConditionsID volitileStatus;
    public List<StatBoost> Boosts {
            get { return boosts; }
        }

        public ConditionsID Status
    {
        get { return status; }
    }

    public ConditionsID VolitileStatus
    {
        get { return volitileStatus; }
    }
}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget moveTar;

    public int Chance
    {
        get { return chance; }
    }

    public MoveTarget MoveTarget
    {
        get { return moveTar; }
    }
}

    [System.Serializable]
    public class StatBoost
    {
        public Stat stat;
        public int boost;
    }


    public enum MoveCatagory
    {
        Physical,
        Special,
        Status
    }

    public enum MoveTarget
    {
        Foe, Self
    }

