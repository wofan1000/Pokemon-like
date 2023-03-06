using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine;

public class GamePartyStates : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
   public static GamePartyStates i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc= owner;

        partyScreen.gameObject.SetActive(true);
        partyScreen.OnSelected += OnCreatureSelected;
        partyScreen.OnBack += OnBack;
    }

    public override void Execute()
    {
        partyScreen.HandleUpdate();
    }

    public override void Exit()
    {
        partyScreen.gameObject.SetActive(false);
        partyScreen.OnSelected -= OnCreatureSelected;
        partyScreen.OnBack -= OnBack;
    }

    void OnCreatureSelected(int selection)
    {
       if( gc.StateMachine.GetPrevState() == InventoryState.i)
        {
            StartCoroutine(GoToUseItemState());
        
        }
        else
        {  
            //to do: open summery screen
            Debug.Log($"selected creature at index  {selection}");
        }
       
    }

    IEnumerator GoToUseItemState()
    {
        yield return gc.StateMachine.PushandWait(UseItemState.i);
        gc.StateMachine.Pop();
    }

     void OnBack()
    {
            gc.StateMachine.Pop();
    }
}
