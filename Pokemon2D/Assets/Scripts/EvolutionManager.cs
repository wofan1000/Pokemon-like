using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField] GameObject evolutionUI;
    [SerializeField] Image creatureImage;

    public event Action OnStartEvolution;
    public event Action OnCompleteEvolution;


    public IEnumerator Evolove(Creature creature, Evolution evolution)
    {
        OnStartEvolution?.Invoke();
        evolutionUI.SetActive(true);

        creatureImage.sprite = creature.Base.RightSprite;
        yield return DialogueManager.Instance.ShowDialogText($" {creature.Base.Name} is changing");

        var oldCreature = creature.Base;
        creature.Evolve(evolution);

        creatureImage.sprite = creature.Base.RightSprite;
        yield return DialogueManager.Instance.ShowDialogText($" {oldCreature.Name} changed into {creature.Base.Name}");

        evolutionUI.SetActive(false);

        OnCompleteEvolution?.Invoke();
    }
}
