using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils.StateMachine;

public class GamePartyStates : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
   public static GamePartyStates i { get; private set; }

    public Creature SelectedCreature { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc= owner;

        SelectedCreature = null;
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
        SelectedCreature = partyScreen.SelectedCreature;

        var prevstate = gc.StateMachine.GetPrevState();

       if( prevstate == InventoryState.i)
        {
            StartCoroutine(GoToUseItemState());
        
        }
       else if (prevstate == BattleStates.i)
        {
            var battleStates = prevstate as BattleStates;


            if (SelectedCreature.HP <= 0)
            {
                return;
            }
            if (SelectedCreature == battleStates.BattleSystem.PlayerUnit.Creature)
           {
               return;
            }
            gc.StateMachine.Pop();
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
        SelectedCreature= null;

        var prevstate = gc.StateMachine.GetPrevState();
        if( prevstate == BattleStates.i)
        {

            var battleState = prevstate as BattleStates;

            if (battleState.BattleSystem.PlayerUnit.Creature.HP <= 0)
            {
                return;
            }
        }
        gc.StateMachine.Pop();
    }
}
