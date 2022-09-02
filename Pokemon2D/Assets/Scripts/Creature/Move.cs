using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public MoveBase Base { get; set; }
    public int MP { get; set; }

    public Move(MoveBase pbase)
    {
        Base = pbase;
        MP = pbase.MP;
    }

    public Move(MoveSaveData savedata)
    {
        Base =  MoveDB.GetMovebyName(savedata.name);
        MP = savedata.mp;
    }

    public MoveSaveData GetSaveData() 
    {
        var savedata = new MoveSaveData()
        {
            name = Base.Name,
            mp = MP
        };
        return savedata;
    }
}

[System.Serializable]
public class MoveSaveData
{
    public string name;
    public int mp;
}
