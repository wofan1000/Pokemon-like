using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemdesc;

    Inventory inventory;

    int selectedItem = 0;

    List<ItemSlotUI> slotUIList;
    private void Awake()
    {
        inventory = Inventory.GetInventory();
    }

    private void Start()
    {
        UpdateItemList();
    }

    void UpdateItemList()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();

        foreach (var itemslot in inventory.Slots)
        {
           var slotuiObj = Instantiate(itemSlotUI, itemList.transform);
            slotuiObj.SetData(itemslot);

            slotUIList.Add(slotuiObj);
        }
        UpdateItemSelection();
    }
    public void HandleUpdate(Action onBack)
    {
        int prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.S))
            ++selectedItem;
        else if (Input.GetKeyDown(KeyCode.W))
            --selectedItem;

        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

        if (prevSelection != selectedItem)
            UpdateItemSelection();

        if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].Nametext.color = GlobalSettings.i.HighlightedColor;
            else
                slotUIList[i].Nametext.color = Color.black;
        }
        var item = inventory.Slots[selectedItem].Item;
        itemIcon.sprite = item.Icon;
        itemdesc.text = item.Description;
    }
}
