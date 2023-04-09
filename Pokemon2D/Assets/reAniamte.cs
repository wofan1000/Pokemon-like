using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class reAniamte : TrainerController
{
    float timeTillReaniated;


    [SerializeField]
    int timeMin = 30, timeMax = 60;
    public override void BattleLost()
    {
        base.BattleLost();
        timeTillReaniated = Random.Range(timeMin, timeMax);
        StartCoroutine(Reanimation());

    }

    public IEnumerator Reanimation ()
    {
        yield return new WaitForSeconds(timeTillReaniated);
        battleLost = false;
        fov.gameObject.SetActive(false);
    }
}
