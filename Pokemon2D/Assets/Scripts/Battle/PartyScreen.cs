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
    Party party;
    int selection = 0;

    public Creature SelectedCreature => creatures[selection];

    public BattleState? CalledFrom { get;  set; }
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

        party = Party.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData ()
    {
        creatures = party.Creatures;

        for(int i = 0; i < memberSlots.Length; i++)
        {
            if (i < creatures.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(creatures[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }
                UpdateMemberSelection(selection);
    }

    public void ShowIfTMIsUsable(TMItems tmItem)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            string message = tmItem.CanBeTaught(creatures[i]) ? "Can Learn" : "Cant Learn";
           // memberSlots[i].SetMessage(message);
        }
    }

    public void ClearMemberSlotMessages()
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            
            memberSlots[i].SetMessage("");
        }
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
