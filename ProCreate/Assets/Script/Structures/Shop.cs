using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, Interactable
{
    [Header("Shop UI")]
    [SerializeField] ShopCanvas Canvas;

    #region Variables for Player in Shop

    bool IsOccupied = false;
    bool PlacingObject = false;

    PlayerMovement Player;

    int PriceToPay = 0;

    #endregion

    #region Components

    BoxCollider box;

    #endregion

    #region Built In functions

    private void Awake()
    { 
        box = this.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(IsOccupied && !PlacingObject)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();
            }
        }
    }

    #endregion;

    #region Interaction

    public void Interact()
    {
        if(!IsOccupied)
        {
            IsOccupied = true;
            IdentifyPlayersUsingShop();
        }
    }

    void IdentifyPlayersUsingShop()
    {
        Collider[] colls;


        colls = Physics.OverlapBox(this.transform.position + transform.forward * (box.center.sqrMagnitude + box.bounds.extents.z), box.bounds.extents, Quaternion.identity, (1 << LayerMask.NameToLayer("Player")));
        if(colls.Length > 0)
        {
            Player = colls[0].GetComponent<PlayerMovement>();
            if(Player != null)
            {
                OpenShop();
            }
        }
    }

    #endregion

    #region Open/Close Shop

    void OpenShop()
    {
        Canvas.ToggleShop();
        Player.UsingShop(true);
    }

    void CloseShop()
    {
        Player.UsingShop(false);
        Player = null;
        Canvas.ToggleShop();
        IsOccupied = false;
    }

    #endregion

    #region Try Purchase Item

    public void TryPurchaseItem(int cost, GameObject ItemPrefab)
    {
        PriceToPay = -1 * cost;
        GameObject PurchasedItem = Instantiate(ItemPrefab);
        ItemMovement ItemMove = PurchasedItem.AddComponent<ItemMovement>();
        ItemMove.Purchased(Player.transform.position, this);
        PlacingObject = true;
        Canvas.ToggleShop();
    }

    public void FinalizePurchase()
    {
        PlayerManager.instance.ChangeMoneyAmount(PriceToPay);
        PlacingObject = false;
        Canvas.ToggleShop();
    }

    public void CancelPurchase()
    {
        PlacingObject = false;
        Canvas.ToggleShop();
    }

    #endregion 

}
