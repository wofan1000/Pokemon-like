using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PhysicalPickup : MonoBehaviour
{

    [SerializeField, HideInInspector] PlayerController player;


    public IEnumerator Interact(Transform initer)
    {
            yield return new WaitForSeconds(.1f);

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.parent = player.transform;
        

    }
}

