using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    [SerializeField] Text moneytext;

    private void Start()
    {
        Wallet.i.OnMoneyChanged += SetMoneyText;
    }
    public void Show()
    {
        gameObject.SetActive(true);
        SetMoneyText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    void SetMoneyText()
    {
        moneytext.text = "g" + Wallet.i.Money;
    }
}
