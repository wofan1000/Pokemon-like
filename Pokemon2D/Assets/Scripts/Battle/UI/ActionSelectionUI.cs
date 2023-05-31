using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.GenericSelection;

public class ActionSelectionUI : SelectionUI<TextSlot>
{
   
    void Start()
    {
        SetSelectionSettings(SelectionType.Grid, 3);
        SetItems(GetComponentsInChildren<TextSlot>().ToList());
    }

  
}
