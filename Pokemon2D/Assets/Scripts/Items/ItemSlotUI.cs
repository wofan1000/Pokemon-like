using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text countText;

    RectTransform rectTransform;


    private void Awake()
    {
       
    }
    public Text Nametext => nameText;

    public Text Counttext => countText;

    public float Hight => rectTransform.rect.height;
    public void SetData(ItemSlot itemslot)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = itemslot.Item.name;
        countText.text = $"X {itemslot.Count}";
    }
    public void SetNameAndPrice(ItemBase item)
    {
        rectTransform = GetComponent<RectTransform>();
        nameText.text = item.name;
        countText.text = $"X {item.Price}";
    }
}
