using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRock : MonoBehaviour
{

   [SerializeField]  PlayerController player, buddy;
   [HideInInspector] public Creature creature, creature2;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        creature = player.i.GetComponentInParent<Party>().creatures[0];
        creature2 = buddy.GetComponentInParent<Party>().creatures[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.up = player.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "BreakableRock" || other.tag == "LavaRock")
        {
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
            creature.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }

        if (other.tag == "Buddy")
        {
            Destroy(this.gameObject);
            creature2.DecreaseHP(creature.MaxHP / creature.MaxHP);
        }

    }
}
