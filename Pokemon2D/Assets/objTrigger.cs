using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objTrigger : MonoBehaviour
{
    [SerializeField] WaterPipe pipe;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "waterPipe" && pipe.isflowing == true)
        {
            Destroy(gameObject);
        }

    }
}
