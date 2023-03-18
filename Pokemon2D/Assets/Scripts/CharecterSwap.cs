using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CharecterSwap : MonoBehaviour
{

    public Transform charecter;
    public List<Transform> charecterstoSwap;

    public int whichCharecter;

    public CinemachineVirtualCamera cam;

    public bool isInRange;
    // Start is called before the first frame update
    void Start()
    {
        if (charecter == null && charecterstoSwap.Count >= 1)
        {
            charecter = charecterstoSwap[0];
        }
        Swap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            whichCharecter = charecterstoSwap.Count - 1;
        }
        else
        {
            whichCharecter -= 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            whichCharecter = 0;
        }
        else
        {
            whichCharecter += 1;
        }
        Swap();
    }

    public void Swap()
    {
        charecter = charecterstoSwap[whichCharecter];
        charecter.GetComponent<Charecter>().enabled = true;
        

        for (int i = 0; i < charecterstoSwap.Count; i++)
        {
            if (charecterstoSwap[i] != charecter)
            {
                charecter.GetComponent<PlayerController>().enabled = false;
                charecter.GetComponent<BuddyController>().enabled = true;
            }
        }
        cam.LookAt = charecter;
        cam.Follow= charecter;
    }


}
