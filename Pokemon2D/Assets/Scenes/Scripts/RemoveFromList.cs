using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromList : MonoBehaviour
{
    public EnemyDoor objToRemove;

    public KillQuest killQuest;

    public EnemyWaterPuzzle enemyPuzzle;
    private void OnDestroy()
    {

        if (objToRemove != null)
        {
            objToRemove.Enemies.Remove(gameObject);
        }


        if (killQuest != null)
        {
            killQuest.Enemies.Remove(gameObject);
        }


        if (enemyPuzzle != null)
        {
            enemyPuzzle.Enemies.Remove(gameObject);
        }
    }
}
