using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CharecterSwap : MonoBehaviour
{

    public ISwitchable charecter;
    public List<Transform> charecterstoSwap;
    /// <summary>
    /// player = true buddy = false
    /// </summary>
    public bool whichCharecter = true;

    public Camera cam;

    public static bool istogether = true;

    public bool isInRange()
    {
        return true;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (charecter == null && charecterstoSwap.Count >= 1)
        {
            //charecter = charecterstoSwap[0];
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            whichCharecter = !whichCharecter;
            
          Swap();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {    
            istogether= !istogether;
        }



    }
    int currentChar(bool character)
    {
        if (character)
        {
            return 0;
        }
        return 1;
    }
    public void Swap()
    {
        if (istogether == true)
        {
            return;
        }

        if (charecterstoSwap[currentChar(whichCharecter)].GetComponent<Charecter>().IsMoving == true)
        {

        }

        charecterstoSwap[currentChar(whichCharecter)].GetComponent<ISwitchable>().OnSwitch(true);
        charecterstoSwap[currentChar(!whichCharecter)].GetComponent<ISwitchable>().OnSwitch(false);

        cam.transform.parent = charecterstoSwap[currentChar(whichCharecter)].GetComponent<ISwitchable>().thecurrentChar;
        cam.transform.localPosition = new Vector3(0, 0, cam.transform.position.z);
        //cam.LookAt = charecterstoSwap[currentChar(whichCharecter)].GetComponent<ISwitchable>().thecurrentChar;
    }

}
