using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour, ISavable, Interactable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialogue dialogue;

    [SerializeField] Animator animator;

    GameState gameState;

    SpriteAnimator spriteAnimator;

    PlayerController playerController;

    
   

    public bool Used { get; set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
 
    public object CaptureState()
    {
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
        {
            animator.SetBool("Opened", true);
        }
    }

    public IEnumerator Interact(Transform initer)
    {
        animator.SetBool("Opened", true);

        //playerController.gameObject.GetComponent<SpriteRenderer>().sprite = playerController.openedChestSprite;

        yield return new WaitForSeconds(0.75f);

        initer.GetComponent<Inventory>().AddItem(item);

        Used = true;


        string playerName = initer.GetComponent<PlayerController>().Name;

        yield return DialogueManager.Instance.ShowDialogText($" {playerName} found {item.Name}");

        gameState = GameState.FreeRoam;
    }

   
}
