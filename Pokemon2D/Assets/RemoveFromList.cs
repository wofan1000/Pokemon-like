using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromList : MonoBehaviour
{
    public EnemyDoor enemies;

    private void OnDestroy()
    {
        if (enemies != null)
        {
            enemies.Enemies.Remove(gameObject);
        }
    }
}
