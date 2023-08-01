using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class CreaterBeastieryEntry
{
    [SerializeField] bool CreatureData;
    [SerializeField] private CreatureBase creature;

    public CreatureBase GetCreatureBase { get { return creature; } }

    public int fought, captured, seen;

   // public CreaterBeastieryEntry(CreatureBase creature)
   // {
    //    this.creature = creature;
   //     CreatureData = false;
   //     seen = 1;

  //  }
}
