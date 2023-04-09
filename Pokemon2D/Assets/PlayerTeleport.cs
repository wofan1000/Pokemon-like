using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;

    
    public  Fader fader;


    private void Awake()
    {
        fader= GetComponent<Fader>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
          
            if (currentTeleporter != null)
            {
                
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().transform.position;
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Teleporter"))
        {
            Debug.Log("is teleported");
            currentTeleporter = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Teleporter"))
        {
            if(other.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }

    
}
