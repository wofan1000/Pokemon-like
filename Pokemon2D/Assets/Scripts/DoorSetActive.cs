using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetActive : MonoBehaviour
{
    [SerializeField]
    public GameObject[] switches;

    public GameObject door;

    int switchpressed = 0;

    private void Update()
    {
        GetSwitchDoor();

    }
    public bool SwitchPress()
    {
        {
            switchpressed = 0;

            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i].GetComponent<DoorSwitch>().isPressed == true)

                    switchpressed++;
            }
        }
        return switches.Length == switchpressed;

    }

    public void GetSwitchDoor()
    {
        door.SetActive(!SwitchPress());

    }

    public void HasKey()
    {

    }
}

