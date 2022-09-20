using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] int xDir;
    [SerializeField] int yDir;


    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    public bool TryToJump(Charecter charecter, Vector2 moveDir)
    {
        if(moveDir.x == xDir && moveDir.y == yDir)
        {
            StartCoroutine(Jump(charecter));
            return true;
        }

        return false;
    }

    IEnumerator Jump(Charecter charecter)
    {
        GameController.instance.PauseGame(true);

        var jumpDestination = charecter.transform.position + new Vector3(xDir, yDir) * 2;
        yield return charecter.transform.DOJump(jumpDestination, 0.3f, 1, 0.5f).WaitForCompletion();

        GameController.instance.PauseGame(false);
    }
}
