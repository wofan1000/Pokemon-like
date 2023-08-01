using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine;

public class UseItemState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    Inventory inventory;

    public bool ItemUsed { get; private set; }
    public static UseItemState i { get; private set; }

    private void Awake()
    {
        i = this;
        inventory = Inventory.GetInventory();
    }
    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        ItemUsed = false;

        StartCoroutine(UseItem());
    }

    IEnumerator UseItem()
    {
        

        yield return HandleTmItems();

        var item = inventoryUI.SelectedItem;
        var creature = partyScreen.SelectedCreature;

        if(item is TMItems)
        {
            yield return HandleTmItems();
        } else
        {
            // Handle Evolution Items
            if (item is EvolutionItem)
            {
                var evolution = creature.CheckForEvolution(item);
                if (evolution != null)
                {
                    yield return GameController.Instance.EvoMan.Evolove(creature, evolution);
                }
                else
                {
                    yield return DialogueManager.Instance.ShowDialogText($"It won't have any affect!");
                    gc.StateMachine.Pop();
                    GameController.Instance.RevertToPrevState();
                    yield break;
                }
            }

            var usedItem = inventory.UseItem(item, partyScreen.SelectedCreature);
            if (usedItem != null)
            {
                ItemUsed = true;

                if (usedItem is RecoveryItem)
                    yield return DialogueManager.Instance.ShowDialogText($"The player used {usedItem.Name}");

                GameController.Instance.RevertToPrevState();
            }
            else
            {
                if (inventoryUI.SelectedCatagory == (int)ItemCatagory.Items)
                    yield return DialogueManager.Instance.ShowDialogText($"It won't have any affect!");
                gc.StateMachine.Pop();
                GameController.Instance.RevertToPrevState();
                yield break;
            }
        }


        gc.StateMachine.Pop();
    }

    IEnumerator HandleTmItems()
    {
        var tmItem = inventoryUI.SelectedItem  as TMItems;
        if (tmItem == null)
            yield break;

        var creature = partyScreen.SelectedCreature;



        if (creature.HasMove(tmItem.Move))
        {
            yield return DialogueManager.Instance.ShowDialogText($"{creature.Base.Name} already knows {tmItem.Move.Name}");
            yield break;
        }

        if (!tmItem.CanBeTaught(creature))
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
}
