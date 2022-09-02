using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

   

    public void SetHP(float hpnormalized)
    {
        health.transform.localScale = new Vector3(hpnormalized, 1f);
    }

    public IEnumerator SmoothHP(float newHP)
    {
        float curHP = health.transform.localScale.x;
        float changeAmt = curHP - newHP;

        while (curHP - newHP > Mathf.Epsilon)
        {
            curHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }

        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
