using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nametext, levelText;
    [SerializeField] HPBar hpBar;

    
    Creature _creature;
    public void SetData(Creature creature)
    {
        nametext.text = creature.Base.Name;
        levelText.text = "Level: " + creature.Level;
        hpBar.SetHP((float)creature.HP / creature.MaxHP);
    }

    public void SetSelected(bool selected)
    {
        if(selected)
            nametext.color = GlobalSettings.i.HighlighedColor;
            else
                nametext.color = Color.black;
        
    }
}
