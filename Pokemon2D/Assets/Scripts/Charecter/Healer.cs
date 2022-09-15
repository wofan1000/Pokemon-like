using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
   public IEnumerator Heal(Transform player, Dialogue dialogue)
    {
        int selectedChoice = 0;

       yield return DialogueManager.Instance.ShowDialogue(dialogue, new List<string>() { "Yes", "No"},
           (choiceIndex) => selectedChoice = choiceIndex);

        if(selectedChoice == 0)
        {
           yield return Fader.i.FadeIn(0.5f);

            var playerParty = player.GetComponent<Party>();
            playerParty.Creatures.ForEach(p => p.Heal());
            playerParty.PartyUpdated();

            yield return Fader.i.FadeOut(0.5f);

            yield return DialogueManager.Instance.ShowDialogText($"You went to sleep, HP restored");
        } else
        {
            yield return DialogueManager.Instance.ShowDialogText($"Okay! Come back anytime.");
        }

       
    }
}
