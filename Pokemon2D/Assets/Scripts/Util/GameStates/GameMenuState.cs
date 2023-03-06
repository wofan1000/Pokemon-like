using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.StateMachine;

public class GameMenuState : State<GameController>
{
    [SerializeField] MenuController menuComtroller;

    public static GameMenuState i { get; private set; }

    private void Awake()
    {
        i = this;
    }
    GameController gameController;

    public override void Enter(GameController owner)
    {
        gameController = owner;
        menuComtroller.gameObject.SetActive(true);
        menuComtroller.OnSelected += OnMenuItemSelected;
        menuComtroller.OnBack += OnBack;
    }

  

    public override void Execute()
    {
        menuComtroller.HandleUpdate();

    }

    public override void Exit()
    {
        menuComtroller.gameObject.SetActive(false);
        menuComtroller.OnSelected -= OnMenuItemSelected;
        menuComtroller.OnBack -= OnBack;
    }

    void OnMenuItemSelected(int selection)
    {
        if (selection == 0)// Creature
            gameController.StateMachine.Push(GamePartyStates.i);
        else if (selection == 1 ) // Inventory
            gameController.StateMachine.Push(InventoryState.i);
    }

    void OnBack()
    {
       gameController.StateMachine.Pop();
    }
}
