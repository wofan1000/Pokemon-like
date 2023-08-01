using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Party : MonoBehaviour
{
    [SerializeField] public List<Creature> creatures;

    public event Action OnUpdated;

    public List<Creature> Creatures
    {
        get { return creatures; 
        }
        set { 
            creatures = value;
            OnUpdated?.Invoke();
        }
        
    }

    private void Awake()
    {
        foreach (var creature in creatures)
        {
            creature.Init();
        }
    }

    private void Start()
    {
       
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
            OnUpdated?.Invoke();
        }
        else
        {
            /// transfer to storage box
        }
    }

    public IEnumerator CheckForEvolutions()
    {
        foreach (var creature in creatures)
        {
          var evolution =  creature.CheckForEvolution();
            if(evolution != null)
            {
                yield return GameController.Instance.EvoMan.Evolove(creature, evolution);
            }
        }
    }

    public void PartyUpdated()
    {
        OnUpdated?.Invoke();
    }

    public static Party GetPlayerParty()
    {
       return FindObjectOfType<PlayerController>().GetComponent<Party>();
    }
}
