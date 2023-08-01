using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questSpawnItem : MonoBehaviour
{

    public GameObject objectToSpawn;

    public Transform spawnPoint;



  
    public void SpawnItem()
    {
        objectToSpawn.transform.position = spawnPoint.position;
        
        if(!objectToSpawn.activeInHierarchy)
        {
            objectToSpawn.SetActive(true);
        }
    }


}
