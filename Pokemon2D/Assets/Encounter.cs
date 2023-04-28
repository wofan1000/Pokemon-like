using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class Encounter : TrainerController

{
   
    public float waitTime = 1f;
    public override void BattleLost()
    {
        base.BattleLost();

        
        StartCoroutine(Destroy());

    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(waitTime);
       Destroy(gameObject);
    }

}
