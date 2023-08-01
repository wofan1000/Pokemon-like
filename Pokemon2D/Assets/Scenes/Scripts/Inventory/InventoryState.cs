using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine;

public class InventoryState : State<GameController> 
{
    [SerializeField] InventoryUI inventoryUI;

    public ItemBase selectedItem { get; private set; }
    public static InventoryState  i { get; private set; }

    Inventory inventory;

    GameController gc;
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public override void Enter(GameController owner)
    {
        gc = owner;

        selectedItem = null;

        inventoryUI.gameObject.SetActive(true);
        inventoryUI.OnSelected += OnItemSelected;
        inventoryUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        inventoryUI.HandleUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        inventoryUI.gameObject.SetActive(false);
        inventoryUI.OnSelected -= OnItemSelected;
        inventoryUI.OnBack -= OnBack;
    }
    public void OnItemSelected(int selection)
    {
        selectedItem = inventoryUI.SelectedItem;
        StartCoroutine(SelectandUseItem());
    }

    public void OnBack()
    {
        selectedItem = null;
        gc.StateMachine.Pop();
    }

    IEnumerator SelectandUseItem()
    {
        var prevState = gc.StateMachine.GetPrevState();

        if(prevState = BattleStates.i)
        {
            // in battle
            if(!selectedItem.CanUseInBattle)
            {
                yield return DialogueManager.Instance.ShowDialogText("Cannot be used in battle");
                yield break;
            }
        } else
        {
            if (!selectedItem.CanUseOutsideBattle)
            {
                yield return DialogueManager.Instance.ShowDialogText("Cannot be used outside battle");
                yield break;
            }

        }
        if (selectedItem is  CapsuleItem) 
        {
            inventory.UseItem(selectedItem, null);
            gc.StateMachine.Pop();
            yield break;
        }
       yield return gc.StateMachine.PushandWait(GamePartyStates.i);

      
        if(prevState == BattleStates.i) 
        {
            if (UseItemState.i.ItemUsed)
                gc.StateMachine.Pop();

            
        }
    }

}
