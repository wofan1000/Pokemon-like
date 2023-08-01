using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/create New TM/HM")]
public class TMItems : ItemBase
{
    [SerializeField] MoveBase move;
    [SerializeField] bool isHM;

    public override string Name => base.Name + $": {move.Name}";

    public override bool Use(Creature creature)
    {
        // Learning move is handled from Inventory UI, If it was learned then return true
        return creature.HasMove(move);
    }

    public bool CanBeTaught(Creature creature)
    {
        return creature.Base.LearnableByItems.Contains(move);
    }

    //public override bool IsReusable => isHM;

    public override bool CanUseInBattle => false;

    public MoveBase Move => move;
    public bool IsHM => isHM;
}
