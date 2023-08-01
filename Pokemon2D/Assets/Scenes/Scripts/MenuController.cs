using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Utils.GenericSelection;

public class MenuController : SelectionUI<TextSlot>
{
    private void Start()
    {
        SetItems(GetComponentsInChildren<TextSlot>().ToList());
    }
}
