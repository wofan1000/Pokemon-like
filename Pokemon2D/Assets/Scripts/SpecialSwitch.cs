using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSwitch : MonoBehaviour
{
    [SerializeField]
    GameObject switchUp;
    [SerializeField]
    GameObject switchDown;

    public bool isPressed = false;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = switchUp.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snowball")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = switchDown.GetComponent<SpriteRenderer>().sprite;
            isPressed = true;
        }


    }
}
