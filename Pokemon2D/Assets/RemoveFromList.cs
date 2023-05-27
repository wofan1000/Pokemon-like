using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromList : MonoBehaviour
{
    public EnemyDoor objToRemove;

  
    private void OnDestroy()
    {
        if (objToRemove != null)
        {
            objToRemove.Enemies.Remove(gameObject);
        }
    }
}
