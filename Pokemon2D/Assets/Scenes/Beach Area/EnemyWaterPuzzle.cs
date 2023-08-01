using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterPuzzle : MonoBehaviour
{
    public List<GameObject> Enemies;

    [SerializeField] WaterPipe pipe;

    private void Update()
    {
        if (Enemies.Count <= 0)
        {
            GetPipe();
        }
    }


    public void GetPipe()
    {
        pipe.isflowing= true;

    }

}
