using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utils.StateMachine;

public enum BattleState
{
    Start,
    ActionSelection,
    MoveSelection,
    RunningTurn,
    Busy,
    PartyScreen,
    BattleOver,
    Inventory
}
public enum BattleAction
{
    Move,
    SwitchCreature,
    UseItem,
    Flee

}

public enum BattleTrigger { Land, Water }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] Battleunit playerUnit;
    [SerializeField] Battleunit enemyUnit;
    [SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] GameObject captureCapsuleSprite;
    [SerializeField] InventoryUI inventoryUI;
    
    [Tooltip("defult attack")]
    [SerializeField] MoveBase genericMove;

  

    [SerializeField] Image battleBackround;
    [SerializeField] Sprite grassBackround, waterBackround;

    

    BattleState state;
    int currentAction;
    int currentMove;
   public int EscapeAttempts { get; set; }

    public event Action<bool> OnBattleOver;

    public int SelectedMove { get;  set; }

    public BattleAction SelectedAction { get; set; }

    public bool isBattleOver { get; private set; }

    public BattleDialogueBox battleDialogueBox { get; private set; }

   public Party playerParty { get; private set; }
   public Party trainerParty { get; private set; }
    public Creature PotentialCreature { get; private set; }

    public Creature SelectedCreature { get;  set; }

    public ItemBase SelectedItem { get; set; }


    public bool IsTrainerBattle { get; private set;} = false;

    PlayerController player;
    TrainerController trainer;

    public StateMachine<BattleSystem> StateMachine { get; private set; }


    [SerializeField] public DamageNumber theDamageNumber { get; private set; }

    BattleTrigger battletrigger;
    // Start is called before the first frame update
    public void StartBattle(Party playerParty, Creature potentialCreature, BattleTrigger trigger = BattleTrigger.Land)
    {
        this.playerParty = playerParty;
        this.PotentialCreature = potentialCreature;
        player = playerParty.GetComponent<PlayerController>();
        IsTrainerBattle = false;
        battletrigger = trigger;

        StartCoroutine(SetUpBattle());
    }

    public void StartTrainerBattle(Party playerParty, Party trainerParty, BattleTrigger trigger = BattleTrigger.Land)
    {
        this.playerParty = playerParty;
        this.trainerParty = trainerParty;

        IsTrainerBattle = true;

        player = playerParty.GetComponent<PlayerController>();
        trainer = trainerParty.GetComponent<TrainerController>();

        battletrigger = trigger;
        StartCoroutine(SetUpBattle());
    }

    public void HandleUpdate()
    {
        StateMachine.Execute();
      
        if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartyScreenSelection();
        }
        else if (state == BattleState.Inventory)
        {
          //  Action onBack = () =>
           // {
           //     inventoryUI.gameObject.SetActive(false);
           //     state = BattleState.ActionSelection;
           // };

          //  Action<ItemBase> onItemUsed = (ItemBase useditem) =>
           // {
           //     StartCoroutine(OnItemUsed(useditem));
           // };

            //inventoryUI.HandleUpdate(onBack);
        }
    }
    void HandlePartyScreenSelection()
    {
        Action onSelected = () =>
        {
            var selectedMember = partyScreen.SelectedCreature;
            if (selectedMember.HP <= 0)
            {
                return;
            }
            if (selectedMember == playerUnit.Creature)
            {
                return;
            }

            partyScreen.gameObject.SetActive(false);

           // if (partyScreen.CalledFrom == BattleState.ActionSelection)
           // {

           //     StartCoroutine(RunTurns(BattleAction.SwitchCreature));
           // }
            //else
            //{
            //    state = BattleState.Busy;
           //     StartCoroutine(SwitchCreature(selectedMember));
           // }
           // partyScreen.CalledFrom = null;
        };

        Action onBack = () =>
        {
            if (playerUnit.Creature.HP <= 0)
            {
                return;
            }
            partyScreen.gameObject.SetActive(false);


            ActionSelection();

           // partyScreen.CalledFrom = null;
        };

       // partyScreen.HandleUpdate(onSelected,onBack);
      
        
    }

    public void BattleOver(bool won)
    {
        isBattleOver = true;
        playerParty.Creatures.ForEach(p => p.OnBattleOver());
        playerUnit.Hud.ClearData();
        enemyUnit.Hud.ClearData();
        OnBattleOver(won);
    }

   public IEnumerator SwitchCreature(Creature Newcreature)
    {

        if (playerUnit.Creature.HP > 0)
        {

            playerUnit.PlayDeathAnim();
            yield return new WaitForSeconds(1f);
        }
        playerUnit.SetUp(Newcreature);
        dialogueBox.SetMoveNames(Newcreature.Moves);

        yield return new WaitForSeconds(1f);

      //  if(partyScreen.CalledFrom == null)
      //  {
       //     state = BattleState.RunningTurn;
       // }
    }

    IEnumerator OnItemUsed(ItemBase usedItem)
    {
        state = BattleState.Busy;
        inventoryUI.gameObject.SetActive(false);

        if (usedItem is CapsuleItem)
        {
            yield return ThrowCapsule((CapsuleItem) usedItem);
        }

       // StartCoroutine(RunTurns(BattleAction.UseItem));
    }
        

   

    private void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.S)) 
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Creature.Moves.Count - 1);

        dialogueBox.UpdateMoveSelection(currentMove, playerUnit.Creature.Moves[currentMove]);
        dialogueBox.UpdateMP(playerUnit.Creature);

        if (Input.GetKeyDown(KeyCode.Z) )
        {
            var move = playerUnit.Creature.Moves[currentMove];
          
            dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableActionSelector(false);

            if (CheckMoveMP(playerUnit, move))
            { 
               playerUnit.Creature.MP -= move.MPCost;
              //StartCoroutine(RunTurns(BattleAction.Move));
            } else
            {
                StartCoroutine(NotEnoughMP());
                
            }

        } else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogueBox.EnableMoveSelector(false);
            ActionSelection();
        }

    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.S))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 4);

        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                dialogueBox.EnableActionSelector(false);
                currentMove = 0;
                //StartCoroutine(RunTurns(BattleAction.Move));
            }
            else if (currentAction == 1)
            {
                MoveSelection();
            }
           // else if (currentAction == 2)
          //  {
          //      OpenInventory();
          //  }
            else if (currentAction == 3)
            {
               // StartCoroutine(RunTurns(BattleAction.Flee));
            }
            else if (currentAction == 4)
            {
               // partyScreen.CalledFrom = state;
                OpenPartyScreen();
            }
        }

    }

    private void OpenPartyScreen()
    {
      //  partyScreen.CalledFrom = state;
        state = BattleState.PartyScreen;
        partyScreen.gameObject.SetActive(true);
    }

    public IEnumerator SetUpBattle()
    {
        StateMachine = new StateMachine<BattleSystem>(this);

        playerUnit.Clear();
        enemyUnit.Clear();

       battleBackround.sprite = (battletrigger == BattleTrigger.Land) ? grassBackround : waterBackround;

        if (!IsTrainerBattle)
        {
            playerUnit.SetUp(playerParty.GetUninjuredCreature());
            enemyUnit.SetUp(PotentialCreature);     
        }
        else
        {
            // send enemy creature
            enemyUnit.gameObject.SetActive(true);
            var enemyCreature = trainerParty.GetUninjuredCreature();
            enemyUnit.SetUp(enemyCreature);

            // send player creature
            playerUnit.gameObject.SetActive(true);
            var playerCreature = playerParty.GetUninjuredCreature();
            playerUnit.SetUp(playerCreature);
            dialogueBox.SetMoveNames(playerUnit.Creature.Moves);
        }
       

        isBattleOver = false;
        EscapeAttempts = 0;
        partyScreen.Init();
        yield return new WaitForSeconds(.1f);

        StateMachine.ChangeState(ActionSelectionState.i);
        
    }



    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogueBox.EnableActionSelector(true);
    }

    void OpenInventory()
    {
        state = BattleState.Inventory;
        inventoryUI.gameObject.SetActive(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableMoveSelector(true);
    }

   

    bool CheckMoveMP(Battleunit sourceUnit, Move moveToCheck)
    {
        if (sourceUnit.Creature.MP >= moveToCheck.MPCost)
        {
            return true;
           // sourceUnit.Creature.MP -= move.MPCost;
           // yield return new WaitForSeconds(1f);
            //StartCoroutine(RunTurns(BattleAction.Move));
        }

        else
        {
            return false;
        }
    }

   

  

        
   
    IEnumerator SendNextTrainerCreature(Creature nextCreature)
    {
        state = BattleState.Busy;

        enemyUnit.SetUp(nextCreature);
        yield return new WaitForSeconds(1f);
        state = BattleState.RunningTurn;
    }
    public IEnumerator ThrowCapsule(CapsuleItem capsuleItem)
    {

        if(IsTrainerBattle)
        {
           
            state = BattleState.RunningTurn;
            yield return DialogueManager.Instance.ShowDialogText("cant capture.");
            yield break;
        }
        yield return new WaitForSeconds(1f);
        var capsuleObj =  Instantiate(captureCapsuleSprite, playerUnit.transform.position - new Vector3(2, 0), Quaternion.identity);
        var capsule = capsuleObj.GetComponent<SpriteRenderer>();
        capsule.sprite = capsuleItem.Icon;

        //animations
        yield return capsule.transform.DOJump(enemyUnit.transform.position + new Vector3(0, 2), 2f, 1, 1f).WaitForCompletion();

        yield return enemyUnit.PlayCaptureAnimation();
        capsule.transform.DOMoveY(enemyUnit.transform.position.y - 1.3f, .5f).WaitForCompletion();

        int shakeCount = TryToCatchCreature(enemyUnit.Creature, capsuleItem);

        for (int i = 0; i < shakeCount; ++i)
        {
            yield return new WaitForSeconds(.5f);
           yield return capsule.transform.DOPunchRotation(new Vector3(0, 0, 10f), .8f).WaitForCompletion();
        }
        if(shakeCount == 4)
        {
            yield return capsule.DOFade(0, 1.5f).WaitForCompletion();
            yield return DialogueManager.Instance.ShowDialogText("creature has been added to ypur party");
            playerParty.AddCreature(enemyUnit.Creature);

            Destroy(capsule);
            BattleOver(true);
        } else
        {
            yield return new WaitForSeconds(1f);
            capsule.DOFade(0, 0.2f);
            yield return enemyUnit.PlayBreakOutAnimation();

            Destroy(capsule);
        }
    }

    public IEnumerator NotEnoughMP()
    {
        yield return DialogueManager.Instance.ShowDialogText($"Not enough MP!");
        GameController.Instance.RevertToPrevState();
        MoveSelection();
    }

    int TryToCatchCreature(Creature creature, CapsuleItem capsuleItem)
    {
        float a = (3 * creature.MaxHP - 2 * creature.HP) * creature.Base.CatchRate * capsuleItem.CatchRateMod
        * ConditionDB.GetStatusBounus(creature.Status) / (3 * creature.MaxHP);

        if (a >= 255)
            return 4;

        float b = 1048560 / MathF.Sqrt(MathF.Sqrt(16711680 / a));

        int shakeCount = 0;

        while(shakeCount < 4)
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
              break;

            ++shakeCount;
        }

        return shakeCount;
    }

  
    public Battleunit PlayerUnit => playerUnit;

    public Battleunit EnemyUnit => enemyUnit;

    public PartyScreen PartyScreen => partyScreen;

    public BattleDialogueBox BattleDialogueBox => battleDialogueBox;

}

