using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {Freeroam, Battle, Dialogue, Cutscene, Paused, Menu, partyscreen, Inventory}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyscreen;
    [SerializeField] InventoryUI inventoryUI;
    TrainerController trainer;

    GameState state;

    GameState  stateBeforePause;

    MenuController menuController;

    public SceneDetails currentScene { get; private set; }

    public SceneDetails prevScene { get; private set; }
    public static GameController instance;
    private void Awake()
    {
        menuController = GetComponent<MenuController>();
        instance = this;
        CreatureDB.init();
        ConditionDB.Init();
        MoveDB.init();
    }


    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;
      
        partyscreen.Init();

        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if( state == GameState.Dialogue)
            state = GameState.Freeroam;
        };

        menuController.onBack += () =>
        {
            state = GameState.Freeroam;
        };

        menuController.onMenuSelected += OnMenuSelected;
    }

    public void PauseGame(bool pause)
    {
        if(pause)
        {
            stateBeforePause = state;
           state = GameState.Paused;
        }
        else
        {
            state = stateBeforePause;
        }
    }

    public void OnEnterTrainerView(TrainerController trainerfov)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattles(playerController));
    }

    void EndBattle(bool won)
    {
        if(trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.Freeroam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

   public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<Party>();
        var potentialCreatures = currentScene.GetComponent<MapArea>().GetRandomCreature();

        var potentalCreatureCopy = new Creature(potentialCreatures.Base, potentialCreatures.Level);

        battleSystem.StartBattle(playerParty, potentalCreatureCopy);
    }

    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        var playerParty = playerController.GetComponent<Party>();
        var trainerParty = trainer.GetComponent<Party>();


        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }


    private void Update()
    {
        if(state == GameState.Freeroam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        if(Input.GetKey(KeyCode.Return))
        {
            menuController.OpenMenu();
            state = GameState.Menu;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            SavingSystem.i.Save("save slot 1");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SavingSystem.i.Load("save slot 1");
        }
       
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        prevScene = currentScene;
        currentScene = currScene;
    }
    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            partyscreen.gameObject.SetActive(true);
            partyscreen.SetPartyData(playerController.GetComponent<Party>().Creatures);
            state = GameState.partyscreen;
        }
        else if (selectedItem == 1)
        {
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Inventory;
        }
        else if (selectedItem == 2)
        {
            SavingSystem.i.Save("save slot 1");
            state = GameState.Freeroam;
        }
        else if (selectedItem == 2)
        {
            SavingSystem.i.Load("save slot 1");
            state = GameState.Freeroam;
        }
      else if (state == GameState.partyscreen)
        {
            Action onSelected = () =>
            {
                // summery screen
            };

            Action onBack = () =>
            {
                partyscreen.gameObject.SetActive(false);
                state = GameState.Freeroam;
            };
            partyscreen.HandleUpdate(onSelected,onBack);
        }
        else if (state == GameState.Inventory)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.Freeroam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }
}
