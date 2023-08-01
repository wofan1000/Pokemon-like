using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerFOV : MonoBehaviour, IPlayerTriggerable
{
    public bool triggerRepeatedly => false;

    public void OnCompanionTriggered(CompanionController companion)
    {
        GameController.Instance.OnEnterTrainersView(GetComponentInParent<TrainerController>());
    }

    public void OnPlayerTriggered(PlayerController player)
    {
        GameController.Instance.OnEnterTrainersView(GetComponentInParent<TrainerController>());
    }
}
