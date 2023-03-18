using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSelection;

public enum BeastieryUIState
{
    HabitateSelection,
    CreatureSelectState,
    Busy
}
public class BeastieryUiState : SelectionUI<TextSlot>
{

    [SerializeField] BeastieryUIState beastieryUI;
   

   
}
