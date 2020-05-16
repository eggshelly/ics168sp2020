using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopCanvas : GeneralCanvas
{
    [Header("Shop Canvas Variables")]
    [SerializeField] TextMeshProUGUI MoneyText;


    public void ToggleShop()
    {
        base.ToggleCanvas();
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        MoneyText.text = "Schmeckles: " + PlayerManager.instance.GetMoneyAmount().ToString();
    }

}
