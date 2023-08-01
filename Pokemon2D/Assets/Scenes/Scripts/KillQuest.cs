using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class KillQuest : MonoBehaviour, ISavable
{

    public List<GameObject> Enemies;


    public GameObject objToSpawn;

    public object CaptureState()
    {
        throw new NotImplementedException();
    }

    public void RestoreState(object state)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (Enemies.Count <= 0)
        {
            objToSpawn.SetActive(true);
        }
    }



}
