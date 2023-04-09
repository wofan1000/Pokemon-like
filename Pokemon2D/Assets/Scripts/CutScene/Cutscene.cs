using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cutscene : MonoBehaviour, IPlayerTriggerable
{
    [SerializeReference]
    [SerializeField] List<CutsceneAction> actions;

    public bool triggerRepeatedly => false;

    public IEnumerator Play()
    {
        GameController.Instance.StartCutsceneState();
        foreach (var action in actions)
        {
            yield return action.Play();
        }
        GameController.Instance.StartFreeRoamState();
    }
    public void AddAction(CutsceneAction action)
    {
        action.Name = action.GetType().ToString();
        actions.Add(action);
    }

    public void OnPlayerTriggered(PlayerController player)
    {
        player.Charecter.Animator.IsMoving= false;
        StartCoroutine(Play());
    }

    public void OnCompanionTriggered(CompanionController companion)
    {
        throw new System.NotImplementedException();
    }
}
