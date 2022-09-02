using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Party : MonoBehaviour
{
    [SerializeField]List<Creature> creatures;

    public List<Creature> Creatures
    {
        get { return creatures; 
        }
        set { creatures = value; }
    }

    private void Start()
    {
        foreach (var creature in creatures)
        {
            creature.Init();
        }
    }

    public Creature GetUninjuredCreature()
    {
      return creatures.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void AddCreature(Creature newCteature)
    {
        if(creatures.Count < 6)
        {
            creatures.Add(newCteature);
        }
        else
        {
            /// transfer to storage box
        }
    }
}
