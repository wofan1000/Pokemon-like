using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamState : Utils.StateMachine.State<GameController>
{

    public static FreeRoamState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gameController;

    public override void Enter(GameController owner)
    {
        gameController = owner;
    }
    public override void Execute()
    {
       // PlayerController.instance.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Return))
            gameController.StateMachine.Push(GameMenuState.i);
    }
}
