using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDB 
{
    static Dictionary<string, MoveBase> moves;

    public static void init()
    {
        moves = new Dictionary<string, MoveBase>();

        var moveList = Resources.LoadAll<MoveBase>("");
        foreach (var move in moveList)
        {
            if (moves.ContainsKey(move.Name))
            {
                continue;
            }


            moves[move.Name] = move;
        }
    }

    public static MoveBase GetMovebyName(string name)
    {
        if (!moves.ContainsKey(name))
        {
            return null;
        }
        return moves[name];
    }
}
