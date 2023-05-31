using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.GenericSelection;

public class MoveSelectionUI : SelectionUI<TextSlot>
{
    [SerializeField] List<TextSlot> moveTextSlots;

    [SerializeField] Text mpText;

    Creature _moves;

    private void Start()
    {
        SetSelectionSettings(SelectionType.Grid, 3);
    }

    private void Update()
    {
        
    }

  

    public void SetMoves(List<Move> moves)
    {
       

        SetItems(moveTextSlots.Take(moves.Count).ToList());

        selectedItem = 0;

        for (int i = 0; i < moveTextSlots.Count; ++i)
        {
            if (i < moves.Count)
                moveTextSlots[i].SetText(moves[i].Base.name);
            else
                moveTextSlots[i].SetText("");
        }
    }

    public override void UpdateSelectionUI()
    {
        base.UpdateSelectionUI();

        var move = _moves;

       // UpdateMP(move);

    }

    void UpdateMP(Creature creatureMp)
    {
        mpText.text = $"MP: {creatureMp.MP}/{creatureMp.MaxMP}";
    }
}
