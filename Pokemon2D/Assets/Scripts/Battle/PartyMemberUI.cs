using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nametext, levelText;
    [SerializeField] HPBar hpBar;

    
    Creature _creature;
    public void Init(Creature creature)
    {
        _creature = creature;
        UpdateData();

        _creature.OnHPChanged += UpdateData;
    }

    void UpdateData()
    {
        nametext.text = _creature.Base.Name;
        levelText.text = "Level: " + _creature.Level;
        hpBar.SetHP((float)_creature.HP / _creature.MaxHP);
    }

    public void SetSelected(bool selected)
    {
        if(selected)
            nametext.color = GlobalSettings.i.HighlighedColor;
            else
                nametext.color = Color.black;
        
    }
}
