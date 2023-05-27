using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourchPuzzleDoor : MonoBehaviour
{
    public GameObject[] Torches;

    int lit = 0;


    public GameObject door;

    private void Update()
    {
        TorchesLit();
        GetTorchDoor();
    }

    public bool TorchesLit()
    {
        {
          lit= 0;

            for (int i = 0; i < Torches.Length; i++)
            {
                if (Torches[i].GetComponent<Torch>().isLit == true)
                
                    lit++;          
            }
            return Torches.Length == lit;
        }

    }
    public void GetTorchDoor()
    {
        door.SetActive(!TorchesLit());

    }

}
