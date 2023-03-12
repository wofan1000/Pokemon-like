using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.StateMachine;

public enum GameState { FreeRoam, Battle, Dialog, Menu, PartyScreen, Inventory, Cutscene, Paused, Evolution, Shop }





public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    [SerializeField] BuddyController buddy;

    [SerializeField] EvolutionManager evoMan;

    GameState state;
    GameState prevState;
    GameState stateBeforeEvolution;

    public StateMachine<GameController> StateMachine { get; private set; }

    public EvolutionManager EvoMan { get => evoMan; }


    public static GameController Instance { get; private set; }

    public BuddyController Buddy { get => buddy; } //set => buddy = value; }

    public PlayerController PlayerController { get => playerController; } //set => playerController = value; }
    private void Awake()
    {
        Instance = this;

        PlayerController.SetBuddy(Buddy);

        CreatureDB.init();
        MoveDB.init();
        ConditionDB.Init();
        ItemDB.init();
        QuestDB.init();
       
    }

    private void Start()
    {
        StateMachine = new StateMachine<GameController>(this);
      
        StateMachine.ChangeState(FreeRoamState.i);

        battleSystem.OnBattleOver += EndBattle;

        partyScreen.Init();

        DialogueManager.Instance.OnShowDialogue += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };


        evoMan.OnStartEvolution += () =>
        {
            stateBeforeEvolution = state;
            state = GameState.Evolution;
        };
        evoMan.OnCompleteEvolution += () =>
        {
            partyScreen.SetPartyData();
            state = stateBeforeEvolution;

            //AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
        };

        ShopController.i.OnStart += () => state = GameState.Shop;
        ShopController.i.OnFinish += () => state = GameState.FreeRoam;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    public void StartBattle(BattleTrigger trigger)
    {
      BattleStates.i.trigger= trigger;
        StateMachine.Push(BattleStates.i);
    }

    TrainerController trainer;
    public void StartTrainerBattle(TrainerController trainer)
    {
        BattleStates.i.trainer = trainer;
        StateMachine.Push(BattleStates.i);

    }

    public void OnEnterTrainersView(TrainerController trainer)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattles(playerController));
    }

    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }

        partyScreen.SetPartyData();

        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        //var playerParty = playerController.creatureparty;
        //bool hasEvolutions = playerParty.CheckForEvolutions();

       
    }

    private void Update()
    {
        StateMachine.Execute();

  
       if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
       

        else if (state == GameState.Shop)
        {
            ShopController.i.HandleUpdate();
        }
    }



    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            // Pokemon
            partyScreen.gameObject.SetActive(true);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1)
        {
            // Bag
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Inventory;
        }
        else if (selectedItem == 2)
        {
            // Save
            SavingSystem.i.Save("saveSlot1");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3)
        {
            // Load
            SavingSystem.i.Load("saveSlot1");
            state = GameState.FreeRoam;
        }
    }

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 24;

        GUILayout.Label("State Stack", style);

        foreach(var state in StateMachine.StateStack)
        {
            GUILayout.Label(state.GetType().ToString(), style);
        }
    }

    public IEnumerator MoveCamera(Vector2 moveOffset, bool waitForFadeOut = false)
    {
        yield return Fader.i.FadeIn(0.5f);

        worldCamera.transform.position += new Vector3(moveOffset.x, moveOffset.y);

        if (waitForFadeOut)
            yield return Fader.i.FadeOut(0.5f);
        else
            StartCoroutine(Fader.i.FadeOut(0.5f));
    }

    public GameState State => state;

    public PlayerController Playercontroller => playerController;

    public Camera WorldCamera => worldCamera;
    public void RevertToPrevState()
    {
        state = prevState;
    }
}
