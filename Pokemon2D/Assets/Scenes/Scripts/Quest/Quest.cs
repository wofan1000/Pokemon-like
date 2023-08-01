using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
     public QuestBase Base { get; private set;  }

    public QuestStatues Status { get; private set; }

    

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectbyName(saveData.name);
        Status = saveData.statues;
    }

    public QuestSaveData GetSaveData()
    {
        var savedata = new QuestSaveData()
        {
            name = Base.Name,
            statues = Status
        };

        return savedata;
    }
    public IEnumerator StartQuest()
    {
        Status = QuestStatues.Started;

       yield return DialogueManager.Instance.ShowDialogue(Base.StartDialogue);

        var questlist = QuestList.GetQuestList();
        questlist.AddQuest(this);
    }

    public IEnumerator CompleteQuest(Transform player)
    {
        Status = QuestStatues.Completed;
      
        yield return DialogueManager.Instance.ShowDialogue(Base.CompletedDialogue);

        var inventory = Inventory.GetInventory();
        if(Base.RequiredItem != null)
        {
            inventory.RemoveItem(Base.RequiredItem);
        }

        if(Base.RewardItem != null)
        {
            inventory.AddItem(Base.RewardItem);

            string playerName = player.GetComponent<PlayerController>().Name;
            yield return DialogueManager.Instance.ShowDialogText($"{playerName} recived {Base.RewardItem.Name}");
        }

        

        var questlist = QuestList.GetQuestList();
        questlist.AddQuest(this);
    }

    public bool CanBeCompleted()
    {
        var inventory = Inventory.GetInventory();
        if(Base.RequiredItem != null)
        {
            if (!inventory.HasItem(Base.RequiredItem))
                return false;
        }
        return true;
    }
}

[System.Serializable]

public class QuestSaveData
{
    public string name;
    public QuestStatues statues;
}
public enum QuestStatues
{
    None,
    Started,
    Completed
}
