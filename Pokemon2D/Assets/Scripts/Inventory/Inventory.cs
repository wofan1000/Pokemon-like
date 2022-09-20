using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ItemCatagory { Items, Capsules, Tms}
public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> capsuleSlots;
    [SerializeField] List<ItemSlot> tmSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;


   

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, capsuleSlots, tmSlots };
    }
    public static List<string> ItemCatagories { get; set; } = new List<string>()
    {
        "Items", "Capsules", "Tms & Hms"

    };

   

    public List<ItemSlot> GetSlotsByCatagory(int catagoryIndex)
    {
        return allSlots[catagoryIndex];
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currenSlots = GetSlotsByCatagory(categoryIndex);
        return currenSlots[itemIndex].Item;
    }

    public ItemBase UseItem(int itemIndex, Creature selectedCreature, int selectedCatagory)
    {
        var item = GetItem(itemIndex, selectedCatagory);
        bool itemused = item.Use(selectedCreature); 

        if(itemused)
        {
            if(!item.IsReusable)
            RemoveItem(item);

            return item;
        }
        return null;
    }

    public void AddItem(ItemBase item, int count = 1)
    {
       int catagory = (int)GetCatagoryFromItem(item);
       var currSlots = GetSlotsByCatagory(catagory);

      var itemslot =  currSlots.FirstOrDefault(slot => slot.Item == item);
        if (itemslot != null)
        {
            itemslot.Count += count;
        }
        else
        {
            currSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }


        OnUpdated?.Invoke();
    }

    public int GetItemCount(ItemBase item)
    {
        
            int catagory = (int)GetCatagoryFromItem(item);
            var currSlots = GetSlotsByCatagory(catagory);

            var itemslot = currSlots.FirstOrDefault(slot => slot.Item == item);

        if (itemslot != null)
           return itemslot.Count;
        else
            return 0;
    }

    public bool HasItem( ItemBase item)
    {
        int catagory = (int)GetCatagoryFromItem(item);
        var currSlots = GetSlotsByCatagory(catagory);

        return currSlots.Exists(slot => slot.Item == item);    
    }
            
    

    public void RemoveItem(ItemBase item, int countToRemove = 1)
    {
        int catagory = (int)GetCatagoryFromItem(item);
        var currSlots = GetSlotsByCatagory(catagory);

        var itemSlot = currSlots.First(slots => slots.Item == item);
        itemSlot.Count -= countToRemove;

        if(itemSlot.Count == 0)
            currSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    ItemCatagory GetCatagoryFromItem(ItemBase item)
    {
        if (item is RecoveryItem || item is EvolutionItem)
            return ItemCatagory.Items;
        else if (item is CapsuleItem)
            return ItemCatagory.Capsules;
        else
            return ItemCatagory.Tms;
    }
    public static Inventory GetInventory()
    {
       return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }

    public object CaptureState()
    {
        var savedata = new InventorySaveData()
        {
            items = slots.Select(i => i.GetSaveData()).ToList(),
            capsules = capsuleSlots.Select(i => i.GetSaveData()).ToList(),
            tms = tmSlots.Select(i => i.GetSaveData()).ToList(),

            
        };

        return savedata;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;

        slots = saveData.items.Select(i => new ItemSlot(i)).ToList();
        capsuleSlots = saveData.capsules.Select(i => new ItemSlot(i)).ToList();
        tmSlots = saveData.tms.Select(i => new ItemSlot(i)).ToList();

        

        allSlots = new List<List<ItemSlot>>() { slots, capsuleSlots, tmSlots };

        OnUpdated?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemSlot()
    {

    }
    public ItemSlot(ItemSaveData savedata)
    {
        item = ItemDB.GetObjectbyName(savedata.name);
        count = savedata.count;
    }
    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            name = item.name,
            count = count
        };

        return saveData;
    }

    public ItemBase Item { 
         get => item;
        set => item = value;

    }

    public int Count {
        get => count;
        set => count = value;
    }
}


[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;
    public List<ItemSaveData> capsules;
    public List<ItemSaveData> tms;
}
