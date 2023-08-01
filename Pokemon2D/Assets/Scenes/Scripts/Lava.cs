using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Lava : MonoBehaviour
{
    [SerializeField] public PlayerController player, buddy;

    public Creature creature, creature2;
   
    private void Start()
    {
        
        creature = player.GetComponentInParent<Party>().creatures[0];
        creature2= buddy.GetComponentInParent<Party>().creatures[0];

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            creature.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }

        if (other.tag == "Buddy")
        {
            creature2.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }
    }
}

   

