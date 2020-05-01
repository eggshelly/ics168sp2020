using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNavigation : MonoBehaviour
{
    #region Components

    [Header("Needed Components")]
    [SerializeField] Shop MainShop;

    ShopGrid Grid;

    #endregion

    int TotalNumItems;
    int SelectedIndex = 0;

    bool ShopOpen = false;
    bool FinishedSetup = false;

    #region Built In Functions

    private void OnEnable()
    {
        if (FinishedSetup)
        {
            Grid.DeselectedItem(SelectedIndex);
            SelectedIndex = 0;
            Grid.SelectedItem(SelectedIndex);
            ShopOpen = true;
        }
    }

    private void OnDisable()
    {
        FinishedSetup = true;
        ShopOpen = false;
    }

    private void Awake()
    {
        Grid = this.GetComponent<ShopGrid>();
    }

    private void Start()
    {
        TotalNumItems = Grid.GetNumItems();
    }

    private void Update()
    {
        if (ShopOpen)
        {
            float verticalInput = InputManager.verticalSelection(MainShop.GetPlayer());

            if (verticalInput > 0)
            {
                PreviousItem();
            }
            else if (verticalInput < 0)
            {
                NextItem();
            }
            else if(InputManager.interact(MainShop.GetPlayer()))
            {
                PurchaseItem();
            }
        }
    }

    #endregion

    #region UI Navigation Functions

    void PreviousItem()
    {
        Grid.DeselectedItem(SelectedIndex);
        SelectedIndex = (SelectedIndex == 0 ? TotalNumItems - 1 : SelectedIndex - 1);
        Grid.SelectedItem(SelectedIndex);
    }

    void NextItem()
    {
        Grid.DeselectedItem(SelectedIndex);
        SelectedIndex = (SelectedIndex + 1 == TotalNumItems ? 0 : SelectedIndex + 1);
        Grid.SelectedItem(SelectedIndex);
    }

    void PurchaseItem()
    {
        if(PlayerManager.instance.HasEnoughMoney(Grid.GetCostOfItem(SelectedIndex)))
        {
            MainShop.TryPurchaseItem(Grid.GetCostOfItem(SelectedIndex), Grid.GetPurchasedItem(SelectedIndex));
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    #endregion


}
