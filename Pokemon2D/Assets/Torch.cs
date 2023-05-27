using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    GameObject Lit;
    [SerializeField]
    GameObject Unlit;

    [SerializeField, HideInInspector]

    CharecterAnimator playerAnim;

    public bool isLit = false;


    [SerializeField, HideInInspector]
    PlayerController player;
    private void Update()
    {
        TRiggerFlame();
     
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.tag == "Torch" && isLit == false))
        {
            isLit = true;
        }

        if (other.gameObject.tag == "Player") 
        {
            if(other.gameObject.GetComponent<CharecterAnimator>().IsSHoldingTorch == true)
            {
                        isLit= true;
            }
        }

    }

    void TRiggerFlame()
    {
        if (isLit == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Lit.GetComponent<SpriteRenderer>().sprite;
           // anim.Play();
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Unlit.GetComponent<SpriteRenderer>().sprite;
            isLit = false;
        }
    }

}
