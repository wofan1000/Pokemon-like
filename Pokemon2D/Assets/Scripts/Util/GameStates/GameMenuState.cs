using System.Collections;
using System.Collections.Generic;
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

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 24;

        GUILayout.Label("State Stack", style);
        
    }

    public override void Execute()
    {
        menuComtroller.HandleUpdate();

        OnBack();
    }

    public override void Exit()
    {
        menuComtroller.gameObject.SetActive(false);
        menuComtroller.OnSelected -= OnMenuItemSelected;
        menuComtroller.OnBack -= OnBack;
    }

    void OnMenuItemSelected(int selected)
    {
        Debug.Log($"selected Menu Item {selected}");
    }

    void OnBack()
    {
       gameController.StateMachine.Pop();
    }
}
