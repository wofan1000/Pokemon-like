using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmashableObject : MonoBehaviour,Interactable
{
    public IEnumerator Interact(Transform initer)
    {
        yield return DialogueManager.Instance.ShowDialogText("This object looks like it can be smashed.");

        var creatureWithCut = initer.GetComponent<Party>().Creatures.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Hit"));

        if (creatureWithCut != null)
        {
            int selectedChoice = 0;

            yield return DialogueManager.Instance.ShowDialogText($"Should {creatureWithCut.Base.name} use Smash?",
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
