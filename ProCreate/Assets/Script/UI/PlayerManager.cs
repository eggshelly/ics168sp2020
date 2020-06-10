using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour, PlayerDefine<PlayerManager.Player>
{
    #region player defination
    public enum Player
    {
        player1,
        player2,
        none
    }
    public Player PlayerEnum { get; set; }
    #endregion

    public static PlayerManager instance = null;

    #region Player Resource Variables
    [Header("Money Variables")]
    [SerializeField] int StartingMoney;
    [SerializeField] int TotalMoney = 0;
    [SerializeField] TextMeshProUGUI MoneyText;

    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TotalMoney = StartingMoney;
        MoneyText.text = string.Format("${0}",TotalMoney);
    }

    #region Money Related Functions

    public int GetMoneyAmount()
    {
        return TotalMoney;
    }

    public void ChangeMoneyAmount(int amount)
    {
        TotalMoney += amount;
        MoneyText.text = string.Format("${0}", TotalMoney);
    }

    public bool HasEnoughMoney(int cost)
    {
        return TotalMoney >= cost;
    }
    #endregion
}
