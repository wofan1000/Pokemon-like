using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text countText;

    public Text Nametext => nameText;

    public Text Counttext => countText;
    public void SetData(ItemSlot itemslot)
    {
        nameText.text = itemslot.Item.name;
        countText.text = $"X {itemslot.Count}";
    }
}
