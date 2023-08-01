using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blockPuzzleHole : MonoBehaviour
{
    [SerializeField]
    GameObject emptyHole;
    [SerializeField]
    GameObject filledHole;

    public bool isFilled = false;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = emptyHole.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            
            gameObject.GetComponent<SpriteRenderer>().sprite = filledHole.GetComponent<SpriteRenderer>().sprite;
            isFilled = true;
            Destroy(GameObject.FindGameObjectWithTag("Block"));
        }
    }
    
}
