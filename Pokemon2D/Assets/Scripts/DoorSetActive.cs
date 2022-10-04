using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour
{

    private bool isopen = false;
   public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    public void ToggleDoor()
    {
        isopen = !isopen;

        if(isopen)
            OpenDoor();
        else
        {
            CloseDoor();
        }

        
    }
}
