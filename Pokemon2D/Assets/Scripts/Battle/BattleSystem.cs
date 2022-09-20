using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    ActionSelection,
    MoveSelection,
    RunningTurn,
    Busy,
    PartyScreen,
    BattleOver,
    Inventory
}
public enum BattleAction
{
    Move,
    SwitchCreature,
    UseItem,
    Flee

}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] Battleunit playerUnit;
    [SerializeField] Battleunit enemyUnit;
    [SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] GameObject captureCapsuleSprite;
    [SerializeField] InventoryUI inventoryUI;

    BattleState state;
    int currentAction;
    int currentMove;
    int escapeAttempts;

    public event Action<bool> OnBattleOver;

    Party playerParty;
    Party trainerParty;
    Creature potentialCreature;

    bool isTrainerBattle = false;

    PlayerController player;
    TrainerController trainer;
    // Start is called before the first frame update
    public void StartBattle(Party playerParty, Creature potentialCreature)
    {
        this.playerParty = playerParty;
        this.potentialCreature = potentialCreature;
        player = playerParty.GetComponent<PlayerController>();
        isTrainerBattle = false;

        StartCoroutine(SetUpBattle());
    }

    public void StartTrainerBattle(Party playerParty, Party trainerParty)
    {
        this.playerParty = playerParty;
        this.trainerParty = trainerParty;

        isTrainerBattle = true;
        player = playerParty.GetComponent<PlayerController>();
        trainer = trainerParty.GetComponent<TrainerController>();
        StartCoroutine(SetUpBattle());
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        } else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        } else if (state == BattleState.PartyScreen)
        {
            HandlePartyScreenSelection();
        }
        else if ( state == BattleState.Inventory)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = BattleState.ActionSelection;
            };

            Action<ItemBase> onItemUsed = (ItemBase useditem) =>
            {
                StartCoroutine(OnItemUsed(useditem));
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }
    void HandlePartyScreenSelection()
    {
        Action onSelected = () =>
        {
            var selectedMember = partyScreen.SelectedCreature;
            if (selectedMember.HP <= 0)
            {
                return;
            }
            if (selectedMember == playerUnit.Creature)
            {
                return;
            }
            partyScreen.gameObject.SetActive(false);

            if (partyScreen.CalledFrom == BattleState.ActionSelection)
            {

                StartCoroutine(RunTurns(BattleAction.SwitchCreature));
            }
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchCreature(selectedMember));
            }
            partyScreen.CalledFrom = null;
        };

        Action onBack = () =>
        {
            if (playerUnit.Creature.HP <= 0)
            {
                return;
            }
            partyScreen.gameObject.SetActive(false);


            ActionSelection();

            partyScreen.CalledFrom = null;
        };

        partyScreen.HandleUpdate(onSelected,onBack);
      
        
    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Creatures.ForEach(p => p.OnBattleOver());
        playerUnit.Hud.ClearData();
        enemyUnit.Hud.ClearData();
        OnBattleOver(won);
    }

    IEnumerator SwitchCreature(Creature Newcreature)
    {

        if (playerUnit.Creature.HP > 0)
        {

            playerUnit.PlayDeathAnim();
            yield return new WaitForSeconds(1f);
        }
        playerUnit.SetUp(Newcreature);
        dialogueBox.SetMoveNames(Newcreature.Moves);

        yield return new WaitForSeconds(1f);

        if(partyScreen.CalledFrom == null)
        {
            state = BattleState.RunningTurn;
        }
    }

    IEnumerator OnItemUsed(ItemBase usedItem)
    {
        state = BattleState.Busy;
        inventoryUI.gameObject.SetActive(false);

        if (usedItem is CapsuleItem)
        {
            yield return ThrowCapsule((CapsuleItem) usedItem);
        }

        StartCoroutine(RunTurns(BattleAction.UseItem));
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

    private void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.S))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentAction, 0, playerUnit.Creature.Moves.Count - 1);

        dialogueBox.UpdateMoveSelection(currentMove, playerUnit.Creature.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var move = playerUnit.Creature.Moves[currentMove];
            if (move.MP <= 0) return;

            dialogueBox.EnableMoveSelector(false);
            StartCoroutine(RunTurns(BattleAction.Move));

        } else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogueBox.EnableMoveSelector(false);
            ActionSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.A))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.S))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 4);

        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                
            }
            else if (currentAction == 1)
            {
                MoveSelection();
            }
            else if (currentAction == 2)
            {
                OpenInventory();
            }
            else if (currentAction == 3)
            {
                StartCoroutine(RunTurns(BattleAction.Flee));
            }
            else if (currentAction == 4)
            {
                partyScreen.CalledFrom = state;
                OpenPartyScreen();
            }
        }

    }

    private void OpenPartyScreen()
    {
        partyScreen.CalledFrom = state;
        state = BattleState.PartyScreen;
        partyScreen.gameObject.SetActive(true);
    }

    public IEnumerator SetUpBattle()
    {
        playerUnit.Clear();
        enemyUnit.Clear();

        if (!isTrainerBattle)
        {
            playerUnit.SetUp(playerParty.GetUninjuredCreature());
            enemyUnit.SetUp(potentialCreature);

            dialogueBox.SetMoveNames(playerUnit.Creature.Moves);
        }
        else
        {
            // send enemy creature
            enemyUnit.gameObject.SetActive(true);
            var enemyCreature = trainerParty.GetUninjuredCreature();
            enemyUnit.SetUp(enemyCreature);

            // send player creature
            playerUnit.gameObject.SetActive(true);
            var playerCreature = playerParty.GetUninjuredCreature();
            playerUnit.SetUp(playerCreature);
            dialogueBox.SetMoveNames(playerUnit.Creature.Moves);
        }
        escapeAttempts = 0;
        partyScreen.Init();
        yield return new WaitForSeconds(1f);
        ActionSelection();
    }



    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogueBox.EnableActionSelector(true);
    }

    void OpenInventory()
    {
        state = BattleState.Inventory;
        inventoryUI.gameObject.SetActive(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableMoveSelector(true);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Creature.CurrentMove = playerUnit.Creature.Moves[currentMove];
            enemyUnit.Creature.CurrentMove = playerUnit.Creature.GetRandomMove();

            bool playerGoesFirst = playerUnit.Creature.Speed >= enemyUnit.Creature.Speed;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondCreature = secondUnit.Creature;

            // first turn
            yield return RunMove(firstUnit, firstUnit, firstUnit.Creature.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver) yield break;

            if (secondCreature.HP > 0)
            {
                // second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Creature.CurrentMove);
                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver) yield break;
            }

        } else
        {
            if (playerAction == BattleAction.SwitchCreature)
            {
                var selectedMember = partyScreen.SelectedCreature;
                state = BattleState.Busy;
                yield return SwitchCreature(selectedMember);
            }
            else if (playerAction == BattleAction.UseItem)
            {
                dialogueBox.EnableActionSelector(false);
                //ThrowCapsule();
            }
            else if (playerAction == BattleAction.Flee)
            {
                yield return TryToEscape();
            }

            // Enemy Turn
            var enemyMove = enemyUnit.Creature.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }
        if (state != BattleState.BattleOver)
            ActionSelection();
    }






    IEnumerator EnemyAttackSelf()
    {
        state = BattleState.RunningTurn;

        var move = enemyUnit.Creature.GetRandomMove();
        yield return RunMove(enemyUnit, enemyUnit, move);

        if (state == BattleState.RunningTurn)
            ActionSelection();

    }

    IEnumerator RunMove(Battleunit sourceUnit, Battleunit tarUnit, Move move)
    {
        bool canRunMove = sourceUnit.Creature.OnBeforeMove();
        if (!canRunMove)
        {
            yield return sourceUnit.Hud.WaitForHpUpdate();
            yield break;
        }

        move.MP--;
        yield return new WaitForSeconds(1f);

        if (CheckIfMoveHits(move, sourceUnit.Creature, tarUnit.Creature))
        {

            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            tarUnit.PLayHitAnimation();

            if (move.Base.Catagory == MoveCatagory.Status)
            {
                RunMoveEffects(move.Base.Effects, sourceUnit.Creature, tarUnit.Creature, move.Base.Target);
            }
            else
            {
                bool isDead = enemyUnit.Creature.TakeDamage(move, sourceUnit.Creature);
                //yield return tarUnit.Hud.UpdateHPAsync();
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

    IEnumerator RunAfterTurn(Battleunit sourceUnit)
    {
        if (state == BattleState.BattleOver)
            yield break;
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        sourceUnit.Creature.OnAfterTurn();
        
        sourceUnit.Hud.WaitForHpUpdate();

        if (sourceUnit.Creature.HP <= 0)
        {
            yield return HandleFaintedUnit(sourceUnit);

            yield return new WaitUntil(() => state == BattleState.RunningTurn);
        }
    }

        IEnumerator HandleFaintedUnit(Battleunit faintedUnit)
    {
        faintedUnit.PlayDeathAnim();

        yield return new WaitForSeconds(2f);

        if(!faintedUnit.IsPlayerUnit)
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
                if(newMove != null)
                {
                    if(playerUnit.Creature.Moves.Count < CreatureBase.maxMoves)
                    {
                        playerUnit.Creature.LearnMove(newMove.Base);
                        dialogueBox.SetMoveNames(playerUnit.Creature.Moves);
                    } else
                    {

                    }
                }

                yield return playerUnit.Hud.SetEXPSmooth(true);
            }

            yield return new WaitForSeconds(1f);
        }

        BattleOverCheck(faintedUnit);
    }
    void BattleOverCheck(Battleunit deadUnit)
    {
        if (deadUnit.IsPlayerUnit)
        {
            var nextCreature = playerParty.GetUninjuredCreature();
            if (nextCreature != null)
                OpenPartyScreen();
            else
                BattleOver(false);
        }
        else
        {
            if (!isTrainerBattle)
            {
                BattleOver(true);
            }
            else
            {
                var nextCreature = trainerParty.GetUninjuredCreature();
                if (nextCreature != null)
                    StartCoroutine(SendNextTrainerCreature(nextCreature));
                else
                    BattleOver(true);
            }

        }
    }
    IEnumerator SendNextTrainerCreature(Creature nextCreature)
    {
        state = BattleState.Busy;

        enemyUnit.SetUp(nextCreature);
        yield return new WaitForSeconds(1f);
        state = BattleState.RunningTurn;
    }
    IEnumerator ThrowCapsule(CapsuleItem capsuleItem)
    {
        state = BattleState.Busy;

        if(isTrainerBattle)
        {
           
            state = BattleState.RunningTurn;
            yield break;
        }
        yield return new WaitForSeconds(1f);
        var capsuleObj =  Instantiate(captureCapsuleSprite, playerUnit.transform.position - new Vector3(2, 0), Quaternion.identity);
        var capsule = capsuleObj.GetComponent<SpriteRenderer>();
        capsule.sprite = capsuleItem.Icon;

        //animations
        yield return capsule.transform.DOJump(enemyUnit.transform.position + new Vector3(0, 2), 2f, 1, 1f).WaitForCompletion();

        yield return enemyUnit.PlayCaptureAnimation();
        capsule.transform.DOMoveY(enemyUnit.transform.position.y - 1.3f, .5f).WaitForCompletion();

        int shakeCount = TryToCatchCreature(enemyUnit.Creature, capsuleItem);

        for (int i = 0; i < shakeCount; ++i)
        {
            yield return new WaitForSeconds(.5f);
           yield return capsule.transform.DOPunchRotation(new Vector3(0, 0, 10f), .8f).WaitForCompletion();
        }
        if(shakeCount == 4)
        {
            yield return capsule.DOFade(0, 1.5f).WaitForCompletion();

            playerParty.AddCreature(enemyUnit.Creature);

            Destroy(capsule);
            BattleOver(true);
        } else
        {
            yield return new WaitForSeconds(1f);
            capsule.DOFade(0, 0.2f);
            yield return enemyUnit.PlayBreakOutAnimation();

            Destroy(capsule);
            state = BattleState.RunningTurn;
        }
    }

    int TryToCatchCreature(Creature creature, CapsuleItem capsuleItem)
    {
        float a = (3 * creature.MaxHP - 2 * creature.HP) * creature.Base.CatchRate * capsuleItem.CatchRateMod
        * ConditionDB.GetStatusBounus(creature.Status) / (3 * creature.MaxHP);

        if (a >= 255)
            return 4;

        float b = 1048560 / MathF.Sqrt(MathF.Sqrt(16711680 / a));

        int shakeCount = 0;

        while(shakeCount < 4)
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
              break;

            ++shakeCount;
        }

        return shakeCount;
    }

    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        if(isTrainerBattle)
        {
            state = BattleState.RunningTurn;
            yield break;
        }
        ++escapeAttempts;

        int playerspeed = playerUnit.Creature.Speed;
        int enemyspeed = enemyUnit.Creature.Speed;

        if(enemyspeed < playerspeed)
        {
            BattleOver(true);
        }
        else
        {
            float f = (playerspeed * 128 / enemyspeed + 30 * escapeAttempts);
            f = f % 256;

            if(UnityEngine.Random.Range(0, 255) < f)
            {
                BattleOver(true);
            }
            else
            {
                state = BattleState.RunningTurn;
            }
        }
    }
}

