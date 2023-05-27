using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;

    public bool Used { get;  set; } = false;

    public bool isPhysicalPickup = false;

    [SerializeField] PlayerController player;

    public object CaptureState()
    {
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if(Used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public IEnumerator Interact(Transform initer)
    {
        if(isPhysicalPickup)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.parent = player.transform;
        }
        if (!Used)
        {
            initer.GetComponent<Inventory>().AddItem(item);

            Used = true;
            
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            string playerName = initer.GetComponent<PlayerController>().Name;

            yield return DialogueManager.Instance.ShowDialogText($" {playerName} found {item.Name}");

            GameController.Instance.RevertToPrevState();
        }
    }

   
}
