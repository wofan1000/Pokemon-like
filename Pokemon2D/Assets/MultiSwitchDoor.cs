using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSwitchDoor : MonoBehaviour
{
    public GameObject[] doorSwitches;

    [SerializeField] private GameObject doorGameObject;

    private Idoor door;

    public Sprite buttonDownSprite;

    public int isPressed = 0;

    private void Awake()
    {
        door = doorGameObject.GetComponent<Idoor>();
        
    }

    private void Update()
    {
        isPressed++;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
            
            if (other.GetComponent<PlayerController>() != null)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = buttonDownSprite;
                isPressed++;
                if(isPressed == doorSwitches.Length)
                {
                    Destroy(doorGameObject);
                }
            
            
        }
    }
}
