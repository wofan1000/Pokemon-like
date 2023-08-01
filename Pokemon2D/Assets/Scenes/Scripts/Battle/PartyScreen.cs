using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.GenericSelection;

public class PartyScreen : SelectionUI<TextSlot> {
    Text message;
    PartyMemberUI[] memberSlots;
    List<Creature> creatures;
    Party party;
   

    public Creature SelectedCreature => creatures[selectedItem];

    

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
        SetSelectionSettings(SelectionType.Grid, 2);

        party = Party.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData()
    {
        creatures = party.Creatures;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < creatures.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(creatures[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }
       var textSlots = memberSlots.Select(m => m.GetComponent<TextSlot>());
        SetItems(textSlots.Take(creatures.Count).ToList());
    }

 

    public void ShowIfTmIsUsable(TMItems tmItem)
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            string message = tmItem.CanBeTaught(creatures[i]) ? "ABLE!" : "NOT ABLE!";
            memberSlots[i].SetMessage(message);
        }
    }

    public void ClearMemberSlotMessages()
    {
        for (int i = 0; i < creatures.Count; i++)
        {
            memberSlots[i].SetMessage("");
        }
    }
}
