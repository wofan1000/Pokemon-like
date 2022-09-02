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

        if (!IsPathClear(targetPos))
            yield break;

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

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1,
           GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer) == true)
            return false;

            return true;
        
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f,  GameLayers.I.SolidLayer | GameLayers.I.InteractableLayer | GameLayers.I.PlayerLayer) != null)
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
