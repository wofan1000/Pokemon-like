using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float pushForce = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D theRB = GetComponent<Rigidbody2D>();

        if((collision.tag == "Player" || collision.tag == "Buddy") && Input.GetKeyDown(KeyCode.Z))
        {
            if(theRB!= null)
            {
                Vector2 Dir = collision.gameObject.transform.position - transform.position;
                Dir.Normalize();

                theRB.AddForce(Dir * pushForce, ForceMode2D.Impulse);

            }
        }
    }
    
        
    
}
