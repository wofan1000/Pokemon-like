using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.GenericSelection;

public class MoveSelectionUI : SelectionUI<TextSlot>
{
    [SerializeField] List<TextSlot> moveSlots;

    [SerializeField] Text mpText;
   public void SetMoves(List<Move> moves)
    {
        SetItems(moveSlots.Take(moves.Count).ToList());

        for (int i = 0; i < moveSlots.Count; ++i)
        {
            if (i < moves.Count)
                moveSlots[i].SetText(moves[i].Base.name);
            else
                moveSlots[i].SetText("");
        }
    }

    public override void UpdateSelectionUI()
    {
        base.UpdateSelectionUI();

       
    }

    public void UpdateMP(Creature creatureMp)
    {
        mpText.text = $"MP: {creatureMp.MP}/{creatureMp.MaxMP}";
    }
}
