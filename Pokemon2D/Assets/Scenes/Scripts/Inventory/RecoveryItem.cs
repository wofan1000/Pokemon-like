using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/create Recovery Items")]
public class RecoveryItem : ItemBase
{
    [Header("HP")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHP;

    [Header("MP")]
    [SerializeField] int mpAmount;
    [SerializeField] bool restoreMaxMP;

    [Header("Status Conditions")]
    [SerializeField] ConditionsID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Revive")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    public override bool Use(Creature creature)
    {
        //revive
        if(revive || maxRevive)
        {
            if (creature.HP > 0)
                return false;

            if (revive)
                creature.IncreaseHP(creature.MaxHP / 2);
            else if (maxRevive)
                creature.IncreaseHP(creature.MaxHP);

            creature.CureStatus();

            return true;

        }

        // no item can be used if fainted
        if (creature.HP == 0)
            return false;

        // restore HP
        if(restoreMaxHP ||  hpAmount > 0)
        {
            if(creature.HP == creature.MaxHP)
                return false;

            if(restoreMaxHP)
                creature.IncreaseHP(creature.MaxHP);
            else
            creature.IncreaseHP(hpAmount);
        }

        if(recoverAllStatus || status != ConditionsID.none)
        {
            if(creature.Status == null && creature.VolitileStatus == null)
                return false;

            if(recoverAllStatus)
            {
                creature.CureStatus();
                creature.CureVolitileStatus();
            }
            else
            {
                if (creature.Status.ID == status)
                    creature.CureStatus();
                else if (creature.VolitileStatus.ID == status)
                    creature.CureVolitileStatus();
                else
                    return false;

            }
        }

        // restoreMP
        if(restoreMaxMP)
        {
            creature.IncreaseMP(mpAmount);
        }
        else
        {
            if(mpAmount > 0)
            {
                creature.IncreaseMP(mpAmount);
            }
        }

        return true;
    }

   
}
