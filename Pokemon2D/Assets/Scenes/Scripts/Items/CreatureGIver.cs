using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGiver : MonoBehaviour, ISavable
{
    [SerializeField] Creature creatureToGive;
    [SerializeField] int count = 1;[SerializeField] Dialogue dialogue;

    bool used = false;

    public IEnumerator GiveCreature(PlayerController player)
    {
        yield return DialogueManager.Instance.ShowDialogue(dialogue);


        creatureToGive.Init();
        player.GetComponent<Party>().AddCreature(creatureToGive);

        used = true;

        string dialoguetext = $"{player.name} recived a {creatureToGive.Base.Name}";
       

        yield return DialogueManager.Instance.ShowDialogText(dialoguetext);
    }

    public bool CanBeGiven()
    {
        return creatureToGive != null  && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}

