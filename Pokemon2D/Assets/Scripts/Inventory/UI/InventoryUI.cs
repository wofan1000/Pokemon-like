using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryUIState 
{ 
    ItemSelection,
    PartySelection,
    Busy
}
public class InventoryUI : MonoBehaviour
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

    int selectedItem = 0;
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

        UpdateItemSelection();
    }

    public void HandleUpdate(Action onBack, Action<ItemBase> onItemUsed = null)
    {
        this.onItemUsed = onItemUsed;

        if (state == InventoryUIState.ItemSelection)
        {
            int prevSelection = selectedItem;
            int prevCategory = selectedCatagory;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                ++selectedItem;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                --selectedItem;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ++selectedCatagory;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                --selectedCatagory;

            if (selectedCatagory > Inventory.ItemCatagories.Count - 1)
                selectedCatagory = 0;
            else if (selectedCatagory < 0)
                selectedCatagory = Inventory.ItemCatagories.Count - 1;

            selectedItem = Mathf.Clamp(selectedItem, 0, inventory.GetSlotsByCatagory(selectedCatagory).Count - 1);

            if (prevCategory != selectedCatagory)
            {
                ResetSelection();
                catagoryText.text = Inventory.ItemCatagories[selectedCatagory];
                UpdateItemList();
            }
            else if (prevSelection != selectedItem)
            {
                UpdateItemSelection();
            }

            if (Input.GetKeyDown(KeyCode.Z))
                StartCoroutine(ItemSelected());
            else if (Input.GetKeyDown(KeyCode.X))
                onBack?.Invoke();
        }
        else if (state == InventoryUIState.PartySelection)
        {
            Action onSelected = () =>
            {
                StartCoroutine(UseItem());
            };

            Action onBackPartyScreen = () =>
            {
                ClosePartyScreen();
            };

            partyScreen.HandleUpdate(onSelected, onBackPartyScreen);
        }
        
    }

    IEnumerator ItemSelected()
    {
        state = InventoryUIState.Busy;

        var item = inventory.GetItem(selectedItem, selectedCatagory);

        if (GameController.instance.State == GameState.Battle)
        {
            // In Battle
            if (!item.CanUseInBattle)
            {
                yield return DialogueManager.Instance.ShowDialogText($"This item cannot be used in battle");
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
            StartCoroutine(UseItem());
        }
        else
        {
            OpenPartyScreen();


            if(item is TMItems)
                partyScreen.ShowIfTMIsUsable(item as TMItems);
            
        }
    }

    IEnumerator UseItem()
    {
        state = InventoryUIState.Busy;

        yield return HandleTmItems();

        var item = inventory.GetItem(selectedItem, selectedCatagory);
        var creature = partyScreen.SelectedCreature;

        // Handle Evolution Items
        if (item is EvolutionItem)
        {
            var evolution = creature.CheckForEvolution(item);
            if (evolution != null)
            {
                yield return EvolutionManager.i.Evolove(creature, evolution);
            }
            else
            {
                yield return DialogueManager.Instance.ShowDialogText($"It won't have any affect!");
                ClosePartyScreen();
                yield break;
            }
        }

        var usedItem = inventory.UseItem(selectedItem, partyScreen.SelectedCreature, selectedCatagory);
        if (usedItem != null)
        {
           
            if (usedItem is RecoveryItem)
                yield return DialogueManager.Instance.ShowDialogText($"The player used {usedItem.Name}");

            onItemUsed?.Invoke(usedItem);
        }
        else
        {
            if (selectedCatagory == (int)ItemCatagory.Items)
                yield return DialogueManager.Instance.ShowDialogText($"It won't have any affect!");
        }

        ClosePartyScreen();
    }

    IEnumerator HandleTmItems()
    {
        var tmItem = inventory.GetItem(selectedItem, selectedCatagory) as TMItems;
        if (tmItem == null)
            yield break;

        var creature = partyScreen.SelectedCreature;

 

        if (creature.HasMove(tmItem.Move))
        {
            yield return DialogueManager.Instance.ShowDialogText($"{creature.Base.Name} already knows {tmItem.Move.Name}");
            yield break;
        }

        if(!tmItem.CanBeTaught(creature))
        {
            yield return DialogueManager.Instance.ShowDialogText($"{creature.Base.Name} cant learn {tmItem.Move.Name}");
            yield break;
        }

        if (creature.Moves.Count < CreatureBase.maxMoves)
        {
            creature.LearnMove(tmItem.Move);
            yield return DialogueManager.Instance.ShowDialogText($"{creature.Base.Name} learned {tmItem.Move.Name}");
        }
    }


    void UpdateItemSelection()
    {
        var slots = inventory.GetSlotsByCatagory(selectedCatagory);

        selectedItem = Mathf.Clamp(selectedItem, 0, slots.Count - 1);

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].Nametext.color = GlobalSettings.i.HighlightedColor;
            else
                slotUIList[i].Nametext.color = Color.black;
        }

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
}
