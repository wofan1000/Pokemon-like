using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Battleunit : MonoBehaviour
{
    [SerializeField] CreatureBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    [SerializeField] BattleHud hud;
    public bool IsPlayerUnit {
        get { return isPlayerUnit; }
    }

    public BattleHud  Hud {
        get{ return hud;  }
        }
    public Creature Creature { get;  set; }

    public Image image;
    Vector3 originalPos;
    Color originalColor;

    public void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.position;
        originalColor = image.color;
    }
    public void SetUp(Creature creature)
    {
        Creature = creature;
        if (isPlayerUnit)
            GetComponent<Image>().sprite = creature.Base.RightSprite;
        else
            GetComponent<Image>().sprite = creature.Base.Leftsprite;

        hud.gameObject.SetActive(true);
        hud.SetData(creature);

        transform.localScale = new Vector3(1, 1, 1);
        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-200, originalPos.x);
        else
            image.transform.localPosition = new Vector3(200f, originalPos.x);

        image.transform.DOLocalMoveY(originalPos.y, 0f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveY(originalPos.y, .25f));
    }

    public void PLayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));

    }

    public void PlayDeathAnim()
    {
        var sequance = DOTween.Sequence();
        sequance.Append(image.transform.DOLocalMoveY(originalPos.y, 0.5f));
        sequance.Join(image.DOFade(0f, 0.5f));
    }

    public IEnumerator PlayCaptureAnimation ()
    {
        var sequance = DOTween.Sequence();
        sequance.Append(image.DOFade(0, .5f));
        sequance.Join(transform.DOLocalMoveY(originalPos.y + 50f, 0.5f));
        sequance.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.5f));
        yield return sequance.WaitForCompletion();
    }

    public IEnumerator PlayBreakOutAnimation()
    {
        var sequance = DOTween.Sequence();
        sequance.Append(image.DOFade(1, .5f));
        sequance.Join(transform.DOLocalMoveY(originalPos.y, 0.5f));
        sequance.Join(transform.DOScale(new Vector3(1f, 1, 1f), 0.5f));
        yield return sequance.WaitForCompletion();
    }
}
