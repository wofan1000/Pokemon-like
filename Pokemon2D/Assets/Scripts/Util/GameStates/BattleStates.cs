using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using Utils.StateMachine;

public class BattleStates : State<GameController>
{
    [SerializeField] BattleSystem Battlesystem;

    public static BattleStates i { get; private set; }

    public BattleTrigger trigger { get; set; }

    public TrainerController trainer { get; set; }

    public LevelManager levelManager;

    private void Awake()
    {
        i = this;
    }
    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        Battlesystem.gameObject.SetActive(true);
        gc.WorldCamera.gameObject.SetActive(false);
        var playerParty = gc.PlayerController.GetComponent<Party>();

        if (trainer == null)
        {
           var potentialCreature = gc.CurrentScene.GetComponent<LevelManager>().GetRandomCreature(trigger);
            var creaturecopy = new Creature(potentialCreature.Base, potentialCreature.Level);
            Battlesystem.StartBattle(playerParty, creaturecopy, trigger);

        }
        else
        {

            var trainerParty = trainer.GetComponent<Party>();
            Battlesystem.StartTrainerBattle(gc.PlayerController.creatureparty, trainer.creatureParty);
        }

        Battlesystem.OnBattleOver += EndBattle;
    }

    public override void Execute()
    {
        Battlesystem.HandleUpdate();
    }

    public override void Exit()
    {
        Battlesystem.gameObject.SetActive(false);
        gc.WorldCamera.gameObject.SetActive(true);

        Battlesystem.OnBattleOver -= EndBattle;
    }
    void EndBattle (bool won)
    {
        if(trainer!= null && won == true) 
        {
            trainer.BattleLost();
            trainer = null;
        }

        gc.StateMachine.Pop();
    }
}
