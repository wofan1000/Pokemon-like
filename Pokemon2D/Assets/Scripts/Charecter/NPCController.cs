using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;
    [SerializeField] List<Vector2> movePattern;
    [SerializeField] float timeBetweenPattern;

    float idleTimer;

    NPCState state;

    int currentMovePattern = 0;

    Quest activeQuest;

    Charecter charecter;
    ItemGiver itemGiver;
    CreatureGiver creatureGiver;

    Healer healer;
    Merchant merchant;

    private void Awake()
    {
        charecter = GetComponent<Charecter>();
        itemGiver = GetComponent<ItemGiver>();
        creatureGiver = GetComponent<CreatureGiver>();
        healer = GetComponent<Healer>();
        merchant = GetComponent<Merchant>();
    }

    public IEnumerator Interact(Transform initer)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialogue;
            charecter.LookTwords(initer.position);

            if (questToComplete != null)
            {
                var quest = new Quest(questToComplete);
                yield return quest.CompleteQuest(initer);
                questToComplete = null;
            }

            if (itemGiver != null && itemGiver.CanBeGiven())
            {
                yield return itemGiver.GiveItem(initer.GetComponent<PlayerController>());
            }
            else if (creatureGiver != null && creatureGiver.CanBeGiven())
            {
                yield return creatureGiver.GiveCreature(initer.GetComponent<PlayerController>());
            }

            else if (questToStart != null)
            {
                activeQuest = new Quest(questToStart);
                yield return activeQuest.StartQuest();
                questToStart = null;


                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initer);
                    activeQuest = null;
                }
            }
            else if (activeQuest != null)
            {
                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initer);
                    activeQuest = null;
                }
                else
                {
                    yield return DialogueManager.Instance.ShowDialogue(activeQuest.Base.InprogressDialogue);
                }
            }
            else if (healer != null)
            {
               yield return healer.Heal(initer,dialogue);
            }
            else if (merchant != null)
            {
               yield return merchant.Trade();
            }
            else
            {
                yield return DialogueManager.Instance.ShowDialogue(dialogue);
            }



            idleTimer = 0;
            state = NPCState.Idle;
        }
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;

                if (movePattern.Count > 0)
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

        if (transform.position != oldPos)
            currentMovePattern = (currentMovePattern + 1) % movePattern.Count;

        state = NPCState.Idle;
    }

    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();
        saveData.activeQuest = activeQuest?.GetSaveData();

        if (questToStart != null)
        saveData.questToStart = (new Quest(questToStart)).GetSaveData();

        if (questToStart != null)
            saveData.questToComplete = (new Quest(questToComplete)).GetSaveData();

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;
        if(saveData != null)
        {
            activeQuest = (saveData.activeQuest != null)? new Quest(saveData.activeQuest) : null;

            questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;

            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;
        }
    }
}


[System.Serializable]
    public class NPCQuestSaveData
    {
        public QuestSaveData activeQuest;
        public QuestSaveData questToStart;
        public QuestSaveData questToComplete;
}


    enum NPCState
    {
        Idle,
        Walk,
        Dialogue
    }

