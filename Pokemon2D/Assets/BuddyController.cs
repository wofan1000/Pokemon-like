using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyController : MonoBehaviour
{
    private Charecter character;

    public void Follow(Vector3 movePosition)
    {
        Vector2 moveVector = movePosition - this.transform.position;
        moveVector = moveVector.Generalize();

        if (!character.IsMoving)
        {
            StartCoroutine(this.character.Move(moveVector, null, true));
        }
    }

    private void Start()
    {
        character = GetComponent<Charecter>();
        this.transform.position = GameController.Instance.PlayerController.transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, GameController.Instance.PlayerController.transform.position) > 3f)
        {
            transform.position = GameController.Instance.PlayerController.transform.position;
        }

        character.HandleUpdate();
    }
}






