using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            door.SetActive(false);
        } else
        {
            door.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
        }
        
    }
}
