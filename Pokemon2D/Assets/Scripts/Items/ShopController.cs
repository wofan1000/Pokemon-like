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
    [SerializeField] ShopUI shopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;
    [SerializeField] Vector2 cameraOffset;

    public event Action OnStart;
    public event Action OnFinish;
    ShopState state;
    public static ShopController i { get; private set; }

    Inventory inventory;

    Merchant merchant;

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
        this.merchant = merchant;
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
            
            yield return GameController.instance.MoveCamera(cameraOffset);
            walletUI.Show();
            shopUI.Show(merchant.AvalibleItems, (item) => StartCoroutine(BuyItem(item)),
               () => StartCoroutine(OnBackFromBuying()));

            state = ShopState.Buying;
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

     IEnumerator OnBackFromBuying()
    {
        yield return GameController.instance.MoveCamera(-cameraOffset);
        shopUI.Close();
        walletUI.Close();
        StartCoroutine(StartMenuState());
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
        else if (state == ShopState.Buying)
            shopUI.HandleUpdate();
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

        walletUI.Show();

        float sellPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventory.GetItemCount(item);

        if(itemCount > 1)
        {
            yield return DialogueManager.Instance.ShowDialogText($"How many would you like to sell?",
                waitForInput: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellPrice, (selectedCount) => countToSell = selectedCount);

            DialogueManager.Instance.CloseDialogue();
        }

        sellPrice = sellPrice * countToSell;

        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogText($"I can do {sellPrice} would you like to sell?",
            waitForInput: false, choices: new List<string>() { "Yes", "No"},
            onchoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if(selectedChoice == 0)
        {
            inventory.RemoveItem(item, countToSell);
            Wallet.i.AddMoney(sellPrice);
            yield return DialogueManager.Instance.ShowDialogText($"You sold {item.Name}");
        }

        walletUI.Close();

        state = ShopState.Selling;

    }

    IEnumerator BuyItem(ItemBase item)
    {
        state = ShopState.Busy;
        yield return DialogueManager.Instance.ShowDialogText($"How many would you like?", waitForInput: false);


        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(100, item.Price, (selectedCount) => countToBuy = selectedCount);

        DialogueManager.Instance.CloseDialogue();

        float totalPrice = item.Price * countToBuy;

        if(Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogueManager.Instance.ShowDialogText($"I can sell it to ya for {totalPrice}",
                waitForInput: false, choices: new List<string>() { "Yes", "No" },
                onchoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if(selectedChoice == 0)
            {
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogueManager.Instance.ShowDialogText($"Thank you, come again!");
            }
        }
        else
        {
            yield return DialogueManager.Instance.ShowDialogText($"Not enough Money for that");
        }

        state = ShopState.Buying;

    }
}
