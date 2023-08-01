using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate_Switch : MonoBehaviour
{
    public GameObject objToRotate;

    [SerializeField]WaterPipe pipe;

    

    
    public DoorSwitch theswitch;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
           StartCoroutine(RotateObject());
        }      
    }

    public IEnumerator RotateObject()
    {     
        objToRotate.transform.Rotate(0, 0, 90);
        pipe.Instance.isInPlace = true;
        yield return new WaitForSeconds(5f);
        objToRotate.transform.Rotate(1f, -3f, -90f);
        pipe.Instance.isInPlace = false;
        theswitch.instance.isPressed= false;
        theswitch.GetComponent<SpriteRenderer>().sprite = theswitch.instance.switchUp.GetComponent<SpriteRenderer>().sprite;
    }  
}
