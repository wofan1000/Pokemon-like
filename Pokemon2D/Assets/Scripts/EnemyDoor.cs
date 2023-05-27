using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public List<GameObject> Enemies;

    [SerializeField] private GameObject doorGameObject;

    private Idoor door;

  

    

    private void Awake()
    {
        door = doorGameObject.GetComponent<Idoor>();

    }

    private void Update()
    {
       if(Enemies.Count <= 0) 
        { 
            Destroy(doorGameObject);
        }
    }
 
}
