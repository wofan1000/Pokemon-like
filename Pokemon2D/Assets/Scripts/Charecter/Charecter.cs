using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charecter : MonoBehaviour
{
    CharecterAnimator animator;
    public float moveSpeed;

    public bool IsMoving { get; private set; }

    public float OffsetY { get; private set; } = 0.3f;
    private void Awake()
    {
        animator = GetComponent<CharecterAnimator>();
        SetPosToTile(transform.position);
    }

    Ledge CheckForLedge(Vector3 tarPos)
    {
        var collider = Physics2D.OverlapCircle(tarPos, 0.15f, GameLayers.I.LedgesLayer);
        return collider?.GetComponent<Ledge>();
    }

    public void SetPosToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f;

        transform.position = pos;
    }
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        var ledge = CheckForLedge(targetPos);

        if(ledge != null)
        {
           if(ledge.TryToJump(this,moveVec))
                yield break;
        }

        if (!IsPathClear(targetPos))
            yield break;


        if(animator.IsSurfing && Physics2D.OverlapCircle(targetPos, 0.3f, GameLayers.I.WaterLayer) == null)
            animator.IsSurfing = false;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;   
    }

    private bool IsPathClear(Vector3 tarPos)
    {
        var diff = tarPos - transform.position;
        var dir = diff.normalized;

        var collisionLayer = GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer;

        if (!animator.IsSurfing)
             collisionLayer = collisionLayer | GameLayers.I.WaterLayer;




        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, collisionLayer) == true)
            return false;

            return true;
        
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f,  GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer) != null)
        {
            return false;
        }

        return true;
    }

    public void LookTwords(Vector3 tarPos)
    {
        var xDiff = Mathf.Floor(tarPos.x) - Mathf.Floor(transform.position.x);
        var yDiff = Mathf.Floor(tarPos.y) - Mathf.Floor(transform.position.y);

        if(xDiff == 0 || yDiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
    }

    public CharecterAnimator Animator
    {
        get => animator;
    }

}
