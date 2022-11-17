using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

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
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

       var wildPokemon = SceneSystem.currentLevelManager.GetRandomCreature(trigger);

        battleSystem.StartBattle(playerController.creatureparty, new Creature(wildPokemon.Base, wildPokemon.Level));
    }

    TrainerController trainer;
    public void StartBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
       

        battleSystem.StartTrainerBattle(playerController.creatureparty,trainer.creatureParty);
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
        //StateMachine.Execute();

        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
               //menuController.OpenMenu();
             state = GameState.Menu;
           }
        }

         if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // TODO: Go to Summary Screen
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Inventory)
        {

            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == GameState.Shop)
        {
            ShopController.i.HandleUpdate();
        }
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
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
    public void RevertToPrevState()
    {
        state = prevState;
    }
}