using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    GameObject Lit;
    [SerializeField]
    GameObject Unlit;

    public bool isLit = false;

    bool canPickup = false;

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

    void Pickup()
    {
        if(canPickup== true)
        {
            gameObject.SetActive(false);
            //gameObject = player.transform.position;
        }
    }

   

}
