using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCanvas : GeneralCanvas
{
    [Header("Shop Canvas Variables")]
    [SerializeField] TextMeshProUGUI MoneyText;


    public void OpenShop()
    {
        base.OpenCanvas();
        UpdateMoneyText();
    }

    public void CloseShop()
    {
        base.CloseCanvas();
    }

    void UpdateMoneyText()
    {
        MoneyText.text = "Wallet: $" + PlayerManager.instance.GetMoneyAmount().ToString();
    }

}
