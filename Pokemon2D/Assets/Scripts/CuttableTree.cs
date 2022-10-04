using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initer)
    {
        yield return DialogueManager.Instance.ShowDialogText("This tree looks like it can be cut.");

        var creatureWithCut = initer.GetComponent<Party>().Creatures.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));

        if (creatureWithCut != null)
        {
            int selectedChoice = 0;

            yield return DialogueManager.Instance.ShowDialogText($"Should {creatureWithCut.Base.name} cut the tree?",
                choices: new List<string>() { "Yes", "No" },
                onchoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                //Yes
                gameObject.SetActive(false);
                yield return DialogueManager.Instance.ShowDialogText($" {creatureWithCut.Base.name} cut the tree");
            }
        }
    }
}

