using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] List<Vector2> movePattern;
    [SerializeField] float timeBetweenPattern;

    float idleTimer;

    NPCState state;

    int currentMovePattern = 0;

    Charecter charecter;

    private void Awake()
    {
        charecter = GetComponent<Charecter>();
    }

    public void Interact(Transform initer)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialogue;
            charecter.LookTwords(initer.position);

            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () =>
            {
                idleTimer = 0;
                state = NPCState.Idle;
            }));
        }
        
    }

    private void Update()
    {
        if(state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;

                if(movePattern.Count > 0)
                 StartCoroutine(charecter.Move(new Vector2(2, 0)));
            }
        }
        charecter.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walk;

        var oldPos = transform.position;

       yield return charecter.Move(movePattern[currentMovePattern]);

        if(transform.position != oldPos)
             currentMovePattern = (currentMovePattern + 1) % movePattern.Count;

        state = NPCState.Idle;
    }

    enum NPCState
    {
        Idle,
        Walk,
        Dialogue
    }
}
