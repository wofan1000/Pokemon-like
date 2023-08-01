using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "LavaRock")
        {
            Destroy(this.gameObject);
        }
    }
}
