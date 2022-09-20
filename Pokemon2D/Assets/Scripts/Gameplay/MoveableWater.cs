using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveableWater : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initer)
    {
        yield return DialogueManager.Instance.ShowDialogText("Yhe water is Deep Blue.");

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

               var animator = initer.GetComponent<CharecterAnimator>();
               var dir = new Vector3(animator.MoveX, animator.MoveY);
                var targetPos = initer.position + dir;

                initer.DOJump(targetPos, 0.3f, 1, 0.5f).WaitForCompletion();
                animator.IsSurfing = true;
            }
        }
    }
}
