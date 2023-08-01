using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialogue dialogue;

    bool used = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogueManager.Instance.ShowDialogue(dialogue);

        player.GetComponent<Inventory>().AddItem(item, count);

        used = true;

        string dialoguetext = $"{player.name} recived a {item.Name}";
        if(count > 1)
           dialoguetext = $"{player.name} recived{count} {item.Name}s";

        yield return DialogueManager.Instance.ShowDialogText(dialoguetext);
    }

    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
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
