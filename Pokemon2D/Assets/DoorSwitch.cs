using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField]
    GameObject switchUp;
    [SerializeField]
    GameObject switchDown;

    public bool isPressed = false;

    public bool ispresuresensative = false;

    



    private void Awake()
    {
        
    }
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUp.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {  
        gameObject.GetComponent<SpriteRenderer>().sprite = switchDown.GetComponent<SpriteRenderer>().sprite;
        isPressed = true;

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (ispresuresensative)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = switchUp.GetComponent<SpriteRenderer>().sprite;

            isPressed = false;
        }
    }
}
