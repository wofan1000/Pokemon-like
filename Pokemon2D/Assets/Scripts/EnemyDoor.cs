using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public GameObject[] Enemies;

    [SerializeField] private GameObject doorGameObject;

    private Idoor door;

  

    public int enemycount = 0;

    private void Awake()
    {
        door = doorGameObject.GetComponent<Idoor>();

    }

    private void Update()
    {
        enemycount--;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.GetComponent<PlayerController>() != null)
        {
            
            enemycount--;
            if (enemycount <= Enemies.Length)
            {
                Destroy(doorGameObject);
            }


        }
    }
}
