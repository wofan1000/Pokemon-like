using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSelection;
using Utils.StateMachine;

public class ActionSelectionState : State<BattleSystem> 
{
    [SerializeField] ActionSelectionUI selectionUI;

    public static ActionSelectionState i { get; private set; }

    BattleSystem bs;

    private void Awake()
    {
        i = this;
    }
    public override void Enter(BattleSystem owner)
    {
        bs = owner;

        selectionUI.gameObject.SetActive(true);
        selectionUI.OnSelected += OnActionSelected;
    }

    public override void Execute()
    {
        selectionUI.HandleUpdate();
    }

    public override void Exit()
    {
        selectionUI.gameObject.SetActive(false);
        selectionUI.OnSelected -= OnActionSelected;
    }

    private void OnActionSelected (int selection) 
    {
        if(selection == 1)
        {
            // magic
            bs.SelectedAction = BattleAction.Move;
            MoveSelectionState.i.Moves = bs.PlayerUnit.Creature.Moves;

            bs.StateMachine.ChangeState(MoveSelectionState.i);
        }
        else if (selection == 2)
        {

        }
        else if (selection == 3)
        {
            //flee
            bs.SelectedAction = BattleAction.Flee;
        }
        else if (selection == 4)
        {
            // party
            StartCoroutine(GoToPartyState());
        }
    }

    IEnumerator GoToPartyState()
    {
       yield return GameController.Instance.StateMachine.PushandWait(GamePartyStates.i);
        var selectedCreature = GamePartyStates.i.SelectedCreature; 
        
        if (selectedCreature != null) 
        {
            bs.SelectedAction = BattleAction.SwitchCreature;
            bs.SelectedCreature = selectedCreature;
            bs.StateMachine.ChangeState(RunTurnState.i);
        }
    }
        
}
