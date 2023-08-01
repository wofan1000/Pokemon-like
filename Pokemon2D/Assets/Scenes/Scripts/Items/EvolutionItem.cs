using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/create new Evolution Item")]
public class EvolutionItem :  ItemBase
{
    public override bool Use(Creature creature)
    {
        return true;
    }
}
