using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed;

    //public Transform target;

    public Vector2 velocity = Vector2.zero;

    public bool IsMoving;
    //private void OnTriggerStay2D(Collider2D collision)
    //{

        
       // if ((collision.tag == "Player" || collision.tag == "Buddy") && Input.GetKeyDown(KeyCode.Z))
       // {
        //    if (collision.tag == "Block")
         //   {
          //      var collider = GameLayers.I.SolidLayer;
          //      transform.position = Vector2.SmoothDamp(transform.position, target.position, ref velocity, speed * Time.deltaTime);
          //  }

       // }
    //}

    public IEnumerator Move(Vector2 moveVec)
    {
       
        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        //var ledge = CheckForLedge(targetPos);
       // if (ledge != null)
       // {
       //     if (ledge.TryToJump(this, moveVec))
       //         yield break;
       // }

        if (!IsPathClear(targetPos))
            yield break;

      
        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        // check if done moving
        //OnMoveOver?.Invoke();
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        var collisionLayer = GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer;

        Debug.DrawLine(transform.position, targetPos, Color.magenta);
        

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, collisionLayer) == true)
            return false;

        return true;
    }



}
