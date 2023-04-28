using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed;

 

    public bool IsMoving;

    public bool canSlid = false;

   

    public IEnumerator Move(Vector2 moveVec)
    {

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;


    // while(canSlid)
       // {
          //  transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
          //  yield return null; 
          //  continue;
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
      
        if(canSlid == true) 
        {
            Move(moveVec);
            
            
        }
        IsMoving = false;

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
