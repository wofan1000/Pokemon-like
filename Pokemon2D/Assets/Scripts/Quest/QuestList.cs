using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestList : MonoBehaviour, ISavable
{
  List<Quest> quests = new List<Quest>();

    public event Action OnUpdated;

    public void AddQuest(Quest quest)
    {
        if (!quests.Contains(quest))
        quests.Add(quest);

        OnUpdated?.Invoke();
    }

    public static QuestList GetQuestList()
    {
        return FindObjectOfType<PlayerController>().GetComponent<QuestList>();
    }

    public bool IsStarted(string questName)
    {
        var questStatus = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus == QuestStatues.Started || questStatus == QuestStatues.Completed;
    }

    public bool IsCompleted(string questName)
    {
        var questStatus = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus  == QuestStatues.Completed;
    }

    public object CaptureState()
    {
       return quests.Select(q => q.GetSaveData()).ToList();
    }

    public void RestoreState(object state)
    {
      var savedata =  state as List<QuestSaveData>;
        if(savedata != null)
        {
            quests = savedata.Select(q => new Quest(q)).ToList();
            OnUpdated?.Invoke();
        }
    }
}
