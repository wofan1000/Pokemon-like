using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveableWater : MonoBehaviour, Interactable, IPlayerTriggerable
{
    bool isJumpingWater = false;

    public bool triggerRepeatedly => true;

    public IEnumerator Interact(Transform initer)
    {
        var animator = initer.GetComponent<CharecterAnimator>();
        if (animator.IsSurfing || isJumpingWater)
            yield break;

        yield return DialogueManager.Instance.ShowDialogText("The water is Deep Blue.");

        var creatureWithSurf = initer.GetComponent<Party>().Creatures.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Surf"));

        if(creatureWithSurf != null)
        {
            int selectedChoice = 0;

            yield return DialogueManager.Instance.ShowDialogText($"Should {creatureWithSurf.Base.name} use surf?",
                choices: new List<string>() { "Yes", "No" },
                onchoiceSelected: (selection) => selectedChoice = selection);

            if(selectedChoice == 0)
            {
                //Yes
                yield return DialogueManager.Instance.ShowDialogText($" {creatureWithSurf.Base.name} used surf");

               
               var dir = new Vector3(animator.MoveX, animator.MoveY);
                var targetPos = initer.position + dir;

                isJumpingWater = true;
                initer.DOJump(targetPos, 0.3f, 1, 0.5f).WaitForCompletion();
                isJumpingWater = false;

                animator.IsSurfing = true;

            } 
            
        }
    }

    public void OnCompanionTriggered(CompanionController companion)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            GameController.Instance.StartBattle(BattleTrigger.Water);
        }
    }


}
