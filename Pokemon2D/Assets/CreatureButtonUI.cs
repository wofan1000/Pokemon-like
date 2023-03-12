using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreatureButtonUI : MonoBehaviour
{
    [SerializeField] Image creatureImage;
     CreaterBeastieryEntry creatureBaseEntrey;
   public void UpdateCreature (CreaterBeastieryEntry currentCreature)
    {
      

        if(currentCreature == null)
        {
            creatureBaseEntrey = null;
            creatureImage.sprite = null;
        } else
        {
            creatureImage.sprite = currentCreature.GetCreatureBase.RightSprite;
            creatureBaseEntrey = currentCreature;
        }
    }
}


