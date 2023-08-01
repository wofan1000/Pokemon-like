using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine;

public class RunTurnState : State<BattleSystem>
{
    public static RunTurnState i { get; private set; }

    BattleSystem bs;

    Battleunit playerUnit;
    Battleunit enemyUnit;

    PartyScreen partyScreen;

    BattleDialogueBox dialoguebox;

    bool isTrainerBattle;

    Party playerParty;
    Party trainerParty;

  
    private void Awake()
    {
        i = this;
    }

    public override void Enter(BattleSystem owner)
    {
        bs = owner;

        playerUnit = bs.PlayerUnit;
        enemyUnit = bs.EnemyUnit;
        dialoguebox = bs.battleDialogueBox;
        partyScreen = bs.PartyScreen;
        isTrainerBattle = bs.IsTrainerBattle;
        playerParty = bs.playerParty;
        trainerParty = bs.trainerParty;

        StartCoroutine(RunTurns(bs.SelectedAction));
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
       

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Creature.CurrentMove = playerUnit.Creature.Moves[bs.SelectedMove];
            enemyUnit.Creature.CurrentMove = enemyUnit.Creature.GetRandomMove();

            bool playerGoesFirst = playerUnit.Creature.Speed >= enemyUnit.Creature.Speed;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondCreature = secondUnit.Creature;

            // first turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Creature.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (bs.isBattleOver) yield break;

            if (secondCreature.HP > 0)
            {
                // second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Creature.CurrentMove);
                yield return RunAfterTurn(secondUnit);
                if (bs.isBattleOver) yield break;
            }

        }
        else
        {
            if (playerAction == BattleAction.SwitchCreature)
            {    
                yield return bs.SwitchCreature(bs.SelectedCreature);
            }
            else if (playerAction == BattleAction.UseItem)
            {
               if(bs.SelectedItem is CapsuleItem)
                {
                   yield return bs.ThrowCapsule(bs.SelectedItem as CapsuleItem);
                    if (bs.isBattleOver) yield break;
                }
                
            }
            else if (playerAction == BattleAction.Flee)
            {
                yield return TryToEscape();
            }

            // Enemy Turn
            var enemyMove = enemyUnit.Creature.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (bs.isBattleOver) yield break;
        }
        if (!bs.isBattleOver)
            bs.StateMachine.ChangeState(ActionSelectionState.i);
    }

    IEnumerator RunMove(Battleunit sourceUnit, Battleunit tarUnit, Move move)
    {
        bool canRunMove = sourceUnit.Creature.OnBeforeMove();
        if (!canRunMove)
        {

            yield return sourceUnit.Hud.WaitForHpUpdate();
            yield break;
        }


        if (CheckIfMoveHits(move, sourceUnit.Creature, tarUnit.Creature))
        {

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(.5f);

            Instantiate(move.Base.attackVisualEffect, tarUnit.transform.position, tarUnit.transform.rotation);
            tarUnit.PLayHitAnimation();


            if (move.Base.Catagory == MoveCatagory.Status)
            {
                RunMoveEffects(move.Base.Effects, sourceUnit.Creature, tarUnit.Creature, move.Base.Target);
            }
            else
            {
                var damageDetails = tarUnit.Creature.TakeDamage(move, sourceUnit.Creature);
                StartCoroutine(ShowDamageNumber(tarUnit.GetComponent<RectTransform>().localPosition, damageDetails));
                //Instantiate(theDamageNumber, tarUnit.transform.position, tarUnit.transform.rotation).SetDamage(damageDetails);
                yield return tarUnit.Hud.WaitForHpUpdate();

            }

            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && tarUnit.Creature.HP > 0)
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rnd = UnityEngine.Random.Range(1, 100);
                    if (rnd < secondary.Chance)
                        RunMoveEffects(secondary, sourceUnit.Creature, tarUnit.Creature, secondary.MoveTarget);
                }
            }
            if (tarUnit.Creature.HP <= 0)
            {
                yield return HandleFaintedUnit(tarUnit);
            }
        }
        else
        {

        }

    }

    bool CheckIfMoveHits(Move move, Creature source, Creature target)
    {
        if (move.Base.AlwaysHits)
            return true;

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 100) <= moveAccuracy;
    }

    IEnumerator RunAfterTurn(Battleunit sourceUnit)
    {
        if (bs.isBattleOver)  yield break;
       
        sourceUnit.Creature.OnAfterTurn();

        sourceUnit.Hud.WaitForHpUpdate();

        if (sourceUnit.Creature.HP <= 0)
        {
            yield return HandleFaintedUnit(sourceUnit);

           
        }
    }

    IEnumerator ShowDamageNumber(Vector3 pos, int damage)
    {
        bs.theDamageNumber.gameObject.SetActive(true);

        bs.theDamageNumber.SetDamage(damage, pos);
        yield return new WaitForSeconds(1f);

        bs.theDamageNumber.gameObject.SetActive(false);

    }

    IEnumerator BattleOverCheck(Battleunit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextCreature = playerParty.GetUninjuredCreature();
            if (nextCreature != null)
            {
                yield return GameController.Instance.StateMachine.PushandWait(GamePartyStates.i);
                yield return bs.SwitchCreature(GamePartyStates.i.SelectedCreature);
            }
            else
                bs.BattleOver(false);
        }
        else
        {
            if (!isTrainerBattle)
            {
                bs.BattleOver(true);
            }
            else
            {
                var nextCreature = trainerParty.GetUninjuredCreature();
                if (nextCreature != null)
                    yield break; //StartCoroutine(SendNextTrainerCreature(nextCreature));
                else
                    bs.BattleOver(true);
            }

        }
    }

    IEnumerator HandleFaintedUnit(Battleunit faintedUnit)
    {
        faintedUnit.PlayDeathAnim();

        yield return new WaitForSeconds(2f);

        if (!faintedUnit.IsPlayerUnit)
        {
            int exp = faintedUnit.Creature.Base.ExpGain;
            int enemylevel = faintedUnit.Creature.Level;
            float trainerBonus = (isTrainerBattle) ? 1.5f : 1f;

            int expGain = Mathf.FloorToInt((exp * enemylevel * trainerBonus) / 7);
            playerUnit.Creature.Exp += expGain;
            yield return playerUnit.Hud.SetEXPSmooth();

            while (playerUnit.Creature.CheckForLevelUp())
            {
                playerUnit.Hud.SetLevel();

                //learn new move
                var newMove = playerUnit.Creature.GetMoveAtLevel();
                if (newMove != null)
                {
                    if (playerUnit.Creature.Moves.Count < CreatureBase.maxMoves)
                    {
                        playerUnit.Creature.LearnMove(newMove.Base);
                        bs.BattleDialogueBox.SetMoveNames(playerUnit.Creature.Moves);
                    }
                    else
                    {

                    }
                }

                yield return playerUnit.Hud.SetEXPSmooth(true);
            }

            yield return new WaitForSeconds(1f);
        }

       yield return BattleOverCheck(faintedUnit);
    }

    void RunMoveEffects(MoveEffects effects, Creature source, Creature target, MoveTarget moveTarget)
    {

        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }

        if (effects.Status != ConditionsID.none)
        {
            target.SetStatus(effects.Status);
        }

        if (effects.VolitileStatus != ConditionsID.none)
        {
            target.SetVolitileStatus(effects.VolitileStatus);
        }

    }

    IEnumerator TryToEscape()
    {
       

        if (isTrainerBattle)
        {
            yield break;
        }
        ++bs.EscapeAttempts;

        int playerspeed = playerUnit.Creature.Speed;
        int enemyspeed = enemyUnit.Creature.Speed;

        if (enemyspeed < playerspeed)
        {
            bs.BattleOver(true);
        }
        else
        {
            float f = (playerspeed * 128 / enemyspeed + 30 * bs.EscapeAttempts);
            f = f % 256;

            if (UnityEngine.Random.Range(0, 255) < f)
            {
                bs.BattleOver(true);
            }
            else
            {
  
            }
        }
    }
}
