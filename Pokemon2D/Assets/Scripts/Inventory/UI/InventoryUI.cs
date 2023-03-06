using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Utils.GenericSelection;

public enum InventoryUIState 
{ 
    ItemSelection,
    PartySelection,
    Busy
}
public class InventoryUI : SelectionUI<TextSlot>
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemdesc;

    [SerializeField] Text catagoryText;


    [SerializeField] PartyScreen partyScreen;

    [SerializeField] Image upArrow, downarrow;

    Action<ItemBase> onItemUsed;

    RectTransform itemListRect;
    Inventory inventory;

    const int itemsInViewport = 8;

   
    int selectedCatagory = 0;
    InventoryUIState state;

    List<ItemSlotUI> slotUIList;
    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateItemList();

        inventory.OnUpdated += UpdateItemList;
    }

    void UpdateItemList()
    {
        // Clear all the existing items
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventory.GetSlotsByCatagory(selectedCatagory))
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }

        SetItems(slotUIList.Select(s => s.GetComponent<TextSlot>()).ToList());

        UpdateSelectionUI();
    }

    public override void HandleUpdate()
    {
        
        int prevCategory = selectedCatagory;

      if (Input.GetKeyDown(KeyCode.D))
            ++selectedCatagory;
        else if (Input.GetKeyDown(KeyCode.A))
            --selectedCatagory;

        if (selectedCatagory > Inventory.ItemCatagories.Count - 1)
            selectedCatagory = 0;
        else if (selectedCatagory < 0)
            selectedCatagory = Inventory.ItemCatagories.Count - 1;

        if (prevCategory != selectedCatagory)
        {
            ResetSelection();
            catagoryText.text = Inventory.ItemCatagories[selectedCatagory];
            UpdateItemList();
        }

        base.HandleUpdate();
    }

    

    IEnumerator ItemSelected()
    {
        state = InventoryUIState.Busy;

        var item = inventory.GetItem(selectedItem, selectedCatagory);

        if (GameController.Instance.State == GameState.Battle)
        {
            // In Battle
            if (!item.CanUseInBattle)
            {
                yield return DialogueManager.Instance.ShowDialogText($"This item cannot be used in battle");
                GameController.Instance.RevertToPrevState();
                state = InventoryUIState.ItemSelection;
                yield break;
            }
        }
        else
        {
            // Outside Battle
            if (!item.CanUseOutsideBattle)
            {
                yield return DialogueManager.Instance.ShowDialogText($"This item cannot be used outside battle");
                state = InventoryUIState.ItemSelection;
                yield break;
            }
        }

        if (selectedCatagory == (int)ItemCatagory.Capsules)
        {
            //StartCoroutine(UseItem());
        }
        else
        {
            OpenPartyScreen();

            if(item is TMItems)
                partyScreen.ShowIfTmIsUsable(item as TMItems);
        
        }
    }

  
    public override void UpdateSelectionUI()
    {
        base.UpdateSelectionUI();

        var slots = inventory.GetSlotsByCatagory(selectedCatagory);

        selectedItem = Mathf.Clamp(selectedItem, 0, slots.Count - 1);

    
        if (slots.Count > 0)
        {
            var item = slots[selectedItem].Item;
            itemIcon.sprite = item.Icon;
            itemdesc.text = item.Description;
        }

        HandleScrolling();
        
    }


    void HandleScrolling()
    {
        if (slotUIList.Count <= itemsInViewport) return;

        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport / 2, 0, selectedItem) * slotUIList[0].Hight;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + itemsInViewport / 2 < slotUIList.Count;
        downarrow.gameObject.SetActive(showDownArrow);
    }

    void ResetSelection()
    {
        selectedItem = 0;

        upArrow.gameObject.SetActive(false);
        downarrow.gameObject.SetActive(false);

        itemIcon.sprite = null;
        itemdesc.text = "";
    }

    void OpenPartyScreen()
    {
        state = InventoryUIState.PartySelection;
        partyScreen.gameObject.SetActive(true);
    }

    void ClosePartyScreen()
    {
        state = InventoryUIState.ItemSelection;

       partyScreen.ClearMemberSlotMessages();
        partyScreen.gameObject.SetActive(false);
    }

    public ItemBase SelectedItem => inventory.GetItem(selectedItem, selectedCatagory);

    public int SelectedCatagory => selectedCatagory;
}
