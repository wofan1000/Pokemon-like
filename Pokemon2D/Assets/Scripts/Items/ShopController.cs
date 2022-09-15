using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ShopState
{   
    Menu,
    Buying,
    Selling,
    Busy
}
public class ShopController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;

    public event Action OnStart;
    public event Action OnFinish;
    ShopState state;
    public static ShopController i { get; private set; }

    Inventory inventory;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }
    public IEnumerator StartTrading(Merchant merchant)
    {
        OnStart?.Invoke();
        yield return StartMenuState();
    }

    public IEnumerator StartMenuState()
    {
        state = ShopState.Menu;
        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogText("Welcome!", waitForInput: false,
            choices: new List<string>() { "Buy", "Sell", "Quit" }, onchoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {

        }
        else if (selectedChoice == 1)
        {
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if(state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }
    IEnumerator SellItem(ItemBase item)
    {
        state = ShopState.Busy;

        if(!item.CanSell)
        {
            yield return DialogueManager.Instance.ShowDialogText($"Cant Sell");
            state = ShopState.Selling;
            yield break;
        }

        float sellPrice = Mathf.Round(item.Price / 2);

        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogText($"I can do {sellPrice} would you like to sell?",
            waitForInput: false, choices: new List<string>() { "Yes", "No"},
            onchoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if(selectedChoice == 0)
        {
            inventory.RemoveItem(item);

        }

        state = ShopState.Selling;

    }
}
