using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleDialogueBox : MonoBehaviour
{
   
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] Color highlightedColor;

    [SerializeField] List<Text> actionText;
    [SerializeField] List<Text> moveText;
    
    [SerializeField] Text mpText;

    [SerializeField] Text mpCostText;

    [SerializeField] CreatureBase creaturebase;



    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for ( int i = 0; i < actionText.Count; ++i)
        {
            if (i == selectedAction)
                actionText[i].color = highlightedColor;
            else
                actionText[i].color = Color.black;
        }

    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveText.Count; ++i)
        {
            if (i == selectedMove)
                moveText[i].color = highlightedColor;
            else
                moveText[i].color = Color.black;
        }
        
        
        mpCostText.text = $"Cost: {move.MPCost} MP";
    }

    public void UpdateMP(Creature creatureMp)
    {
        mpText.text = $"MP: {creatureMp.MP}/{creatureMp.MaxMP}";
    }
   

    public void SetMoveNames(List<Move> moves)
    {
        for(int i = 0; i < moveText.Count; ++i)
        {
            if (i < moves.Count)
                moveText[i].text = moves[i].Base.name;
            else
                moveText[i].text = "";
        }
    }
}
