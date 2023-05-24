using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : TrainerController, Interactable
{
    float timeTillReaniated;

    public SpriteRenderer reanimate;

    public bool isalive, isreanimating;

    [SerializeField]
    int timeMin = 30, timeMax = 60;

    [SerializeField]
    Behaviour movementScript;

    public int amounttoUse;
    public override void BattleLost()
    {
        base.BattleLost();

        timeTillReaniated = Random.Range(timeMin, timeMax);
        StartCoroutine(Reanimation());

    }


    public IEnumerator Reanimation()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = reanimate.GetComponent<SpriteRenderer>().sprite;
        isreanimating = true;
        isalive = false;
        movementScript.enabled = false;
        yield return new WaitForSeconds(timeTillReaniated);

        movementScript.enabled = true;
        isalive = true;
        isreanimating = false;
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


    public IEnumerator Smash(Transform initer)
    {
        if (isreanimating)
        {
            yield return DialogueManager.Instance.ShowDialogText("This tree looks like it can be cut.");

            var creatureWithCut = initer.GetComponent<Party>().Creatures.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));

            if (creatureWithCut != null)
            {
                int selectedChoice = 0;

                yield return DialogueManager.Instance.ShowDialogText($"Should {creatureWithCut.Base.name} cut the tree?",
                    choices: new List<string>() { "Yes", "No" },
                    onchoiceSelected: (selection) => selectedChoice = selection);

                if (selectedChoice == 0)
                {
                    //Yes
                    gameObject.SetActive(false);
                    amounttoUse++;
                }
            }
        }
    }
}

