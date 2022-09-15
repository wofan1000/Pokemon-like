using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Create New Quest")]
public class QuestBase : ScriptableObject
{
    [SerializeField]string name;
    [SerializeField] string desctription;

    [SerializeField] Dialogue startDialogue;
    [SerializeField] Dialogue inProgressDialogue;
    [SerializeField] Dialogue completedDialogue;

    [SerializeField] ItemBase requiredItem;
    [SerializeField] ItemBase rewardItem;

    public string Name => name;

    public string Description => desctription;

   public Dialogue StartDialogue => startDialogue;

   public Dialogue InprogressDialogue => inProgressDialogue?.Lines.Count > 0 ? inProgressDialogue : startDialogue;

   public Dialogue CompletedDialogue => completedDialogue;

    public ItemBase RequiredItem => requiredItem;

    public ItemBase RewardItem => rewardItem;
}
