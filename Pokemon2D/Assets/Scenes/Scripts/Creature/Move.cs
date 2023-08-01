using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public MoveBase Base { get; set; }
    public int MPCost { get; set; }

   public AttackEffect attackVisual { get; set; }

    public Move(MoveBase pbase)
    {
        Base = pbase;
        MPCost = pbase.MPCost;

        attackVisual = pbase.attackVisualEffect;
    }

    public Move(MoveSaveData savedata)
    {
        Base = MoveDB.GetObjectbyName(savedata.name);
        MPCost = savedata.mpCost;
    }

    public MoveSaveData GetSaveData() 
    {
        var savedata = new MoveSaveData()
        {
            name = Base.name,
            mpCost = MPCost
        };
        return savedata;
    }

 
}

[System.Serializable]
public class MoveSaveData
{
    public string name;
    public int mpCost;
}
