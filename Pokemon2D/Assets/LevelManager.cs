using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] SceneDetails levelRef;

    public SceneDetails levelRefs{ get { return levelRef; } }

    private void Awake()
    {
        levelRef.SetLevelManager(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"Entered {gameObject.name}");


           StartCoroutine(SceneSystem.EnterLevel(this));


        }
        Debug.Log("test");
    }
    
}
