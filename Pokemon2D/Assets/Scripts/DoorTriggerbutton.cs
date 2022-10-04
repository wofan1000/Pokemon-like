using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerbutton : MonoBehaviour
{
    PlayerController player;
    public bool playerInRange;
    [SerializeField] private DoorSetActive door;

    private void Update()
    {
        player = GetComponent<PlayerController>();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(playerInRange)
            door.OpenDoor();
            playerInRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
            playerInRange = true;
        else
        {
            playerInRange = false;
        }
    }
}
