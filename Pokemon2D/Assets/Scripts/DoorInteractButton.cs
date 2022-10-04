using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractButton : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;
    private Idoor door;

    private float timer;

    public Sprite buttonDownSprite;

    private void Awake()
    {
        door = doorGameObject.GetComponent<Idoor>();
    }

    private void Update()
    {
        if( timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                door.CloseDoor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = buttonDownSprite;
            Destroy(doorGameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            timer = 1f;
        }
    }

}
