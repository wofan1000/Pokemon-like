using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nametext, levelText, statusText;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject expBar;

    [SerializeField] Color psnColor, SlpColor, frzColor, parColor, brnColor;

    Creature _creature;

    Dictionary<ConditionsID, Color> statusColors; 
    public void SetData(Creature creature)
    {
        _creature = creature;

        nametext.text = creature.Base.Name;
        SetLevel();
        hpBar.SetHP((float)creature.HP / creature.MaxHP);
        SetEXP();

       statusColors = new Dictionary<ConditionsID, Color>()
        {
            {ConditionsID.psn, psnColor },
            {ConditionsID.slp, SlpColor },
            {ConditionsID.frz, frzColor },
            {ConditionsID.par, parColor },
            {ConditionsID.burn, brnColor},
        };


        SetStatusText();
        _creature.OnStatusChanged += SetStatusText;
       
    }
    public IEnumerator UpdateHP()
    {
        if (_creature.HPChanged)
        {
            yield return hpBar.SmoothHP((float)_creature.HP / _creature.MaxHP);
            _creature.HPChanged = false;
        }
    }

    public void SetLevel()
    {
        levelText.text = "lvl" + _creature.Level;
    }


    void SetStatusText()
    {
      if(_creature.Status == null)
        {
            statusText.text = "";
        } else
        {
            statusText.text = _creature.Status.ID.ToString().ToUpper();
            statusText.color = statusColors[_creature.Status.ID];
        }
    }

    public void SetEXP()
    {
        if (expBar == null) return;

      float normalizedExp =  GetNormalizedEXP();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
       
    }

    public IEnumerator SetEXPSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if(reset)
         expBar.transform.localScale = new Vector3(0, 1, 1);


        float normalizedExp = GetNormalizedEXP();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();

    }

    float GetNormalizedEXP()
    {
        int currentlevelExp = _creature.Base.GetExpForLevel(_creature.Level);
        int nextlevelExp = _creature.Base.GetExpForLevel(_creature.Level + 1);

        float normalizedExp =   (float)(_creature.Exp - currentlevelExp) / (nextlevelExp - currentlevelExp);
        return Mathf.Clamp01(normalizedExp);
    }
    
}
