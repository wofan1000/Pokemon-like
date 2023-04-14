using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockPuzzleDoor : MonoBehaviour
{

    [SerializeField]
    public GameObject[] Holes;


    public GameObject door;

    int holeFilled = 0;

  
    void Update()
    {
        GetHoleDoor();
    }

    public bool BlockFilled()
    {
        {
            holeFilled = 0;

            
            for (int i = 0; i < Holes.Length; i++)
            {
                if (Holes[i].GetComponent<blockPuzzleHole>().isFilled == true)

                    holeFilled++;
            }
        }
        return Holes.Length == holeFilled;

    }

    public void GetHoleDoor()
    {
        door.SetActive(!BlockFilled());

    }

}
