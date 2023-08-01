using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorSetActive : MonoBehaviour
{
    [SerializeField]
    public GameObject[] switches;

    public GameObject door;

    int switchpressed = 0;

   public bool iscode;

    public bool iskey;

    [SerializeField]  public int[] combonation;

   public int currentIndex = 0;

    [HideInInspector]
    public DoorSetActive instance;

    private void Awake()
    {
        instance = this;
    }
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

   public void CodePress(int buttonNumber)
    {
        if (iscode)
        {
            if (buttonNumber != combonation[currentIndex])
            {
                currentIndex = 0;
                return;
            }
          
            
        }
        if(++currentIndex >= combonation.Length)
        {
            GetSwitchDoor();
        }
    }

}

