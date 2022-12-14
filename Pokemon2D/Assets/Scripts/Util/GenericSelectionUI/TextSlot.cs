using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] Text text;

    Color originalColor;

  

    private void Awake()
    {
        originalColor = text.color;
    }
    public void OnSelectionChanged(bool isSelected)
    {
        text.color = (isSelected) ? GlobalSettings.i.HighlighedColor : originalColor;
    }

  
}
