using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class HoldItem : MonoBehaviour
{
    public Transform holdSopt;
    public LayerMask pickupMask;

    public Vector3 dir { get;  set; }
    public GameObject isholding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(isholding)
            {
                isholding.transform.position = transform.position + dir;

                isholding.transform.parent = null;

                if (isholding.GetComponent<Rigidbody2D>())
                    isholding.GetComponent<Rigidbody2D>().simulated = true;
                isholding = null;
            } else
            {
              Collider2D pickupItem =  Physics2D.OverlapCircle(transform.position + dir, .4f, pickupMask);

                if(pickupItem)
                {
                    isholding = pickupItem.gameObject;
                    isholding.transform.position = holdSopt.position;
                    isholding.transform.parent = transform;
                    if(isholding.GetComponent<Rigidbody2D>())
                        isholding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }
}
