using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSwitch : MonoBehaviour
{
    public Transform objects;

    public Transform spawnpoint;


    [SerializeField]
    GameObject switchUp;
    [SerializeField]
    GameObject switchDown;

    public bool ispresuresensative = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            objects.position = spawnpoint.position;
            gameObject.GetComponent<SpriteRenderer>().sprite = switchDown.GetComponent<SpriteRenderer>().sprite;
            
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (ispresuresensative)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = switchUp.GetComponent<SpriteRenderer>().sprite;

            }
        }
    }
    }
    
