using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour
{
    Text message;
    PartyMemberUI[] memberSlots;
    List<Creature> creatures;

    int selection = 0;

    public Creature SelectedCreature => creatures[selection];

    public BattleState? CalledFrom { get;  set; }
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
    }

    public void SetPartyData (List<Creature> creatures)
    {
        this.creatures = creatures;
        for(int i = 0; i < memberSlots.Length; i++)
        {
            if (i < creatures.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].SetData(creatures[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }
                UpdateMemberSelection(selection);
    }

    public void HandleUpdate(Action onSelected, Action onBack)
    {
        var prevSelection = selection;

        if (Input.GetKeyDown(KeyCode.D))
            ++selection;
        else if (Input.GetKeyDown(KeyCode.A))
            --selection;
        else if (Input.GetKeyDown(KeyCode.S))
            selection += 2;
        else if (Input.GetKeyDown(KeyCode.W))
            selection -= 2;

        selection = Mathf.Clamp(selection, 0, creatures.Count - 1);

        if(selection != prevSelection)
        UpdateMemberSelection(selection);

        if (Input.GetKeyDown(KeyCode.Z))
        {
         onSelected();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
          onBack();
        }
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

  
}
