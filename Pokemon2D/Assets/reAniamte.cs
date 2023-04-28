using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class reAniamte : TrainerController, Interactable
{
    float timeTillReaniated;

    public SpriteRenderer reanimate;

    public bool isalive, isreanimating, isboss;

    [SerializeField]
    int timeMin = 30, timeMax = 60;

    [SerializeField]    
    Behaviour movementScript;
    public override void BattleLost()
    {
        base.BattleLost();
        
        timeTillReaniated = Random.Range(timeMin, timeMax);
        StartCoroutine(Reanimation());
      
    }

    public IEnumerator Reanimation ()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = reanimate.GetComponent<SpriteRenderer>().sprite;
        isreanimating= true;
        isalive = false;
        movementScript.enabled= false;
        yield return new WaitForSeconds(timeTillReaniated);

        movementScript.enabled = true;
        isalive = true;
        isreanimating= false;
        battleLost = false;
        fov.gameObject.SetActive(true);
        StartCoroutine(Heal());
       
    }

    public IEnumerator Heal()
    {
           
            yield return new WaitForSeconds(.1f);

            var party = GetComponent<Party>();
            party.Creatures.ForEach(p => p.Heal());
            party.PartyUpdated();

        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Torch" && isreanimating == true)
        {
            Destroy(gameObject);
        }
       
    }

  
    }

    
