using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    [SerializeField] Creature creature;

    [SerializeField] int DMGtoGive;
        

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            creature.DecreaseHP(creature.HP - DMGtoGive);
        }
    }
}
