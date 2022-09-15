using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItem : MonoBehaviour, IPlayerTriggerable
{

    [SerializeField] Dialogue dialogue;

    

    public void OnPlayerTriggered(PlayerController player)
    {
        player.Charecter.Animator.IsMoving = false;
        DialogueManager.Instance.ShowDialogue(dialogue);
    }
    public bool triggerRepeatedly => false;

    
}
