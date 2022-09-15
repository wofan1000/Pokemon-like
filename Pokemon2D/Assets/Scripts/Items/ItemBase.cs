using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] float price;
    [SerializeField] bool canSell;

    public string Name => name;

    public string Description => description;

    public Sprite Icon => icon;

    public float Price => price;

    public bool CanSell => canSell;

    public virtual bool Use(Creature creature)
    {
        return false;
    }
}
