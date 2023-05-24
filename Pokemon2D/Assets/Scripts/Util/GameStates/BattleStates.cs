using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils.StateMachine;

public class BattleStates : State<GameController>
{
    [SerializeField] BattleSystem battleSystem;

    public TrainerController trainer { get; set; }

    public static BattleStates i { get; private set; }

    public BattleTrigger trigger { get; set; }


    
    void Awake()
    {
        i = this;

   
    }

    GameController gc;
 
    public override void Enter(GameController owner)
    {
        gc = owner;

        battleSystem.gameObject.SetActive(true);
        gc.WorldCamera.gameObject.SetActive(false);

        var playerParty = gc.Playercontroller.GetComponent<Party>();

        
        if (trainer == null)
        {
            var potentialCreatures = SceneSystem.currentLevelManager.GetRandomCreature(trigger);
           

            var potentalCreatureCopy = new Creature(potentialCreatures.Base, potentialCreatures.Level);

            battleSystem.StartBattle(playerParty, potentalCreatureCopy, trigger);
        } else
        {

            var trainerParty = trainer.GetComponent<Party>();
              battleSystem.StartTrainerBattle(playerParty, trainerParty);
        }

        battleSystem.OnBattleOver += EndBattle;

    }


    public override void Exit()
    {
        battleSystem.gameObject.SetActive(false);
        gc.WorldCamera.gameObject.SetActive(true);

        battleSystem.OnBattleOver -= EndBattle;
    }

    public override void Execute()
    {
        battleSystem.HandleUpdate();
    }

    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }

        gc.StateMachine.Pop();
    }

    public BattleSystem BattleSystem => battleSystem;
}
