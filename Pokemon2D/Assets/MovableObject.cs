using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed;

    public Transform target;

    public Vector2 velocity = Vector2.zero;
    private void OnTriggerStay2D(Collider2D collision)
    {
       

        if((collision.tag == "Player" || collision.tag == "Buddy") && Input.GetKeyDown(KeyCode.Z))
        {
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref velocity, speed * Time.deltaTime);
        }
    }
    
        
    
}
