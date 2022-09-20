using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemdesc;

    int selectedItem;

    RectTransform itemListRect;

    [SerializeField] Image upArrow, downarrow;

    List<ItemBase> avalibleItems;

    List<ItemSlotUI> slotUIList;

    Action<ItemBase> onItemSelected;

    Action onBack;

    const int itemsInViewport = 8;




    private void Awake()
    {
        
        itemListRect = GetComponent<RectTransform>();
    }

    public void Show(List<ItemBase> avalibleItems, Action<ItemBase> onItemSelected, Action onBack)
    {
        this.avalibleItems = avalibleItems;
        this.onItemSelected = onItemSelected;
        this.onBack = onBack;

        gameObject.SetActive(true);
        UpdateItemList();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void HandleUpdate()
    {
        var prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.S))
            ++selectedItem;
        else if (Input.GetKeyDown(KeyCode.W))
            --selectedItem;

        selectedItem = Mathf.Clamp(selectedItem, 0, avalibleItems.Count - 1);

        if (selectedItem != prevSelection)
            UpdateItemSelection();
    }

    void UpdateItemList()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();

        foreach (var item in avalibleItems)
        {
            var slotuiObj = Instantiate(itemSlotUI, itemList.transform);
            slotuiObj.SetNameAndPrice(item);

            slotUIList.Add(slotuiObj);
        }
        UpdateItemSelection();

        if (Input.GetKeyDown(KeyCode.Z))
            onItemSelected?.Invoke(avalibleItems[selectedItem]);
        else if (Input.GetKeyDown(KeyCode.X))
            onBack?.Invoke();
    }

    void UpdateItemSelection()
    {
        

        selectedItem = Mathf.Clamp(selectedItem, 0, avalibleItems.Count - 1);

        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].Nametext.color = GlobalSettings.i.HighlightedColor;
            else
                slotUIList[i].Nametext.color = Color.black;
        }

        if (avalibleItems.Count > 0)
        {
            var item = avalibleItems[selectedItem];
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

}
