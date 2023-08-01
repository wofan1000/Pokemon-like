using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils.StateMachine;

public class CutsceneState : State<GameController> 
{
   public static CutsceneState i { get; private set; }

    private void Awake()
    {
        i = this;
    }
}
