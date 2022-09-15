using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/create New Capsule")]
public class CapsuleItem : ItemBase
{
    [SerializeField] float catchRateMod = 1;
    public override bool Use(Creature creature)
    {
        if(GameController.instance.State == GameState.Battle)
            return true;

        return false;
    }

    public float CatchRateMod => catchRateMod;
}
