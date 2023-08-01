using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] List<ItemBase> avalibleItems;
  public IEnumerator Trade()
    {
       yield return ShopController.i.StartTrading(this);
    }

    public List<ItemBase> AvalibleItems => avalibleItems;
}
