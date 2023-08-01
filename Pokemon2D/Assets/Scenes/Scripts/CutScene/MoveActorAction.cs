using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MoveActorAction : CutsceneAction
{
    [SerializeField] Charecter charecter;
    [SerializeField] List<Vector2> movePattern;

    public override IEnumerator Play()
    {
        foreach(var moveVec in movePattern)
        {
           yield return charecter.Move(moveVec);
        }
    }
}
