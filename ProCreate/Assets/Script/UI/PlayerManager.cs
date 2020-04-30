using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    #region Player Resource Variables
    [Header("Money Variables")]
    [SerializeField] int StartingMoney;


    [SerializeField] int TotalMoney = 0;

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
    }

    #region Money Related Functions

    public int GetMoneyAmount()
    {
        return TotalMoney;
    }

    public void ChangeMoneyAmount(int amount)
    {
        TotalMoney += amount;
    }

    public bool HasEnoughMoney(int cost)
    {
        return TotalMoney >= cost;
    }
    #endregion
}
