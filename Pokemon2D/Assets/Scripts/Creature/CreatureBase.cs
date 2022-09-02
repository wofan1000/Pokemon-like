using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Creature/Create New")]
public class CreatureBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    [SerializeField] CreatureType type1;
    [SerializeField] CreatureType type2;

    //base stats

    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int maxhp;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
   
    [SerializeField] int expGain;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] int catchRate = 255;

    public static int maxMoves { get; set; }

    [SerializeField] List<LearnableMoves> learnableMoves;

    public int GetExpForLevel(int level)
    {
        if(growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }
        else if (growthRate == GrowthRate.MediumFast)
        {
            return level * level * level;
        }

        return -1;
    }
    public string Name
    {
        get { return name; }
    }

    public string Description { 
        
        get { return description; } 
    }
    public Sprite Leftsprite
    {
        get { return leftSprite; }
    }

    public Sprite RightSprite
    {
        get { return rightSprite; }
    }
    public CreatureType Type1
    {
        get { return type1; }
    }
    public CreatureType Type2
    {
        get { return type2; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int MaxHP
    {
        get { return maxhp; }
    }

    public List<LearnableMoves> LearnableMoves
    {
        get { return learnableMoves; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public int CatchRate => catchRate;

    public int ExpGain => expGain;

    public GrowthRate GrowthRate => growthRate;



}

[System.Serializable] 
public class LearnableMoves
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get { return moveBase; }

        }

    public int Level
    {
        get { return level; }
    }

  

}


public enum CreatureType
{
    none,
    normal,
    fire,
    water,
    darkness,
    ice,
    poison
}

public enum GrowthRate
{
    Fast,
    MediumFast,
}
public enum Stat
{
    Attack,
    Defense,
    SpDefense,
    SpAttack,
    Speed,

    // to boost move accuracy
    Accuracy,
    Evasion,
    
}



public class TypeChart
{
   static float[][] chart =
    {
        //                    NOR  FIR  WAT
       /* NOR*/ new float [] {1f,   1f, 1f},
       /* FIR*/ new float [] {1f,  .5f,.5f},
       /* WAT*/ new float [] {1f,   2f,.5f}
    };

    public static float GetEffectivness(CreatureType attackType, CreatureType defenseType)
    {
        if (attackType == CreatureType.none || defenseType == CreatureType.none)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}