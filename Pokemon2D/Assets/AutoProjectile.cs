using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoProjectile : MonoBehaviour
{
    
    [SerializeField] PlayerController player, buddy;
    public Creature creature, creature2;
    public Vector2 tarPos;
    public float speed;
    




    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerController>();

        buddy = FindObjectOfType<PlayerController>();


        creature = player.GetComponentInParent<Party>().creatures[0];
        creature2 = buddy.GetComponentInParent<Party>().creatures[0];
    }

    private void Update()
    {
          transform.position = Vector2.MoveTowards(transform.position, tarPos, speed * Time.deltaTime);
       
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BreakableRock" || other.tag == "LavaRock")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            creature.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }

        if (other.tag == "Buddy")
        {
            Destroy(this.gameObject);
            creature.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }

    }
}



 

