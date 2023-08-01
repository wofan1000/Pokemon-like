using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballMove : MonoBehaviour
{
    public float speed;



    public bool IsMoving;

    public bool canSlid = false;



    public IEnumerator Move(Vector2 moveVec)
    {
        {
            var targetPos = transform.position;
            IsMoving = true;
            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return null;

            }
            transform.position = targetPos;

            IsMoving = false;

        }
    }

    public Vector2 CheckLocation(Vector2 moveVec, Vector2 currentPos)
    {
        //currentPos += moveVec;
        var testPos = currentPos + moveVec;
        if (IsPathClear(testPos))
        {
            testPos = CheckLocation(moveVec, testPos);
            return testPos;
        }
        return currentPos;
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        var collisionLayer = GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer | GameLayers.I.PlayerLayer;

        //Debug.DrawLine(transform.position, targetPos, Color.magenta);


        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, collisionLayer) == true)
            return false;

        return true;
    }

}

