using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDB : MonoBehaviour
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionsID = kvp.Key;
            var condition = kvp.Value;

            condition.ID = conditionsID;
        }
    }
    
    public static Dictionary<ConditionsID, Condition> Conditions { get; set; } = new Dictionary<ConditionsID, Condition>()
    {
        {
            ConditionsID.psn,
            new Condition()
            {
                Name = "Poison",
                OnAfterTurn = (Creature creature) =>
                {
                    creature.DecreaseHP(creature.MaxHP/8);
                }
            }
        },
         {
            ConditionsID.burn,
            new Condition()
            {
                Name = "Burn",
                OnAfterTurn = (Creature creature) =>
                {
                    creature.DecreaseHP(creature.MaxHP/15);
                }
            }
        },
          {
            ConditionsID.par,
            new Condition()
            {
                Name = "Paralize",
               OnBeforeMove = (Creature creature) =>
               {
                  if ( Random.Range(0, 5) == 1)
                   {
                       return false;
                   }
                   return true;
               }
            }
        },
        {
            ConditionsID.frz,
            new Condition()
            {
                Name = "Freeze",
               OnBeforeMove = (Creature creature) =>
               {
                  if ( Random.Range(0, 5) == 1)
                   {
                       creature.CureStatus();
                       return false;
                   }
                   return false;
               }
            }
        },
        {
            ConditionsID.slp,
            new Condition()
            {
                Name = "Sleep",
                 OnStart = (Creature creature) =>
                 {
                     creature.StatusTime = Random.Range(1,4);
                 },
               OnBeforeMove = (Creature creature) =>
               {

                   if(creature.StatusTime <= 0)
                   {
                       creature.CureStatus();
                       return true;
                   }
                 creature.StatusTime--;
                   
                   return false;
               }
            }
        },
        {
            ConditionsID.confusion,
            new Condition()
            {
                Name = "Confusion",
                 OnStart = (Creature creature) =>
                 {
                     creature.StatusTime = Random.Range(1,5);
                 },
               OnBeforeMove = (Creature creature) =>
               {

                   if(creature.VolitileStatusTime <= 0)
                   {
                       creature.CureVolitileStatus();
                       return true;
                   }
                 creature.VolitileStatusTime--;

                   return false;
               }
            }
        }
    };

    public static float GetStatusBounus(Condition condition)
    {
        if(condition == null)
            return 1f;
        else if(condition.ID == ConditionsID.slp || condition.ID == ConditionsID.frz)
            return 1f;
        else if (condition.ID == ConditionsID.par || condition.ID == ConditionsID.psn || condition.ID == ConditionsID.burn)
            return 1.5f;

        return 1f;
    }
}

public enum ConditionsID
{
   none, psn, burn, slp, par,frz, confusion
}
