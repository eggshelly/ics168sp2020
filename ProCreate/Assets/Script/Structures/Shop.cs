using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, Interactable
{
    [Header("Shop UI")]
    [SerializeField] ShopCanvas Canvas;

    [Header("Objects For Camera")]
    [SerializeField] GameObject CameraLookAtTrans;

    #region Variables for Player in Shop

    bool IsOccupied = false;
    bool PlacingObject = false;

    PlayerMovement Player;

    GameObject PurchasedItem;

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
        if (IsOccupied && !PlacingObject)
        {
            if(InputManager.cancel(Player.GetPlayer()))
            {
                Debug.Log("Player left shop");
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
            for(int i = 0; i < colls.Length; ++i)
            {
                if((Player = colls[0].GetComponent<PlayerMovement>()) != null)
                {
                    Debug.Log("Person using shop");
                    OpenShop();
                }
            }
        }
    }

    #endregion

    #region Open/Close Shop

    void OpenShop()
    {
        Debug.Log("Opening Shop");
        SmoothCamera.instance.SwapTransform(CameraLookAtTrans.transform, Player.gameObject.transform, true);
        Canvas.OpenShop();
        Player.UsingShop(true);
        Debug.Log("Shop opened");
    }

    void CloseShop()
    {
        Debug.Log("Closing Shop");
        SmoothCamera.instance.SwapTransform(CameraLookAtTrans.transform, Player.gameObject.transform, false);
        Canvas.CloseShop();
        Player.UsingShop(false);
        IsOccupied = false;
        Debug.Log("Shop closed");
    }

    #endregion

    #region Try Purchase Item

    public void TryPurchaseItem(int cost, GameObject ItemPrefab)
    {
        PriceToPay = -1 * cost;
        PurchasedItem = Instantiate(ItemPrefab);
        ItemMovement ItemMove = PurchasedItem.AddComponent<ItemMovement>();
        ItemMove.Purchased(Player.transform.position, this);
        PlacingObject = true;
        SmoothCamera.instance.SwapTransform(PurchasedItem.transform, CameraLookAtTrans.transform, true);
        Canvas.CloseShop();
    }

    public void FinalizePurchase()
    {
        PlayerManager.instance.ChangeMoneyAmount(PriceToPay);
        PlacingObject = false;
        SmoothCamera.instance.SwapTransform(PurchasedItem.transform, CameraLookAtTrans.transform, false);
        PurchasedItem = null;
        Canvas.OpenShop();
    }

    public void CancelPurchase()
    {
        SmoothCamera.instance.SwapTransform(PurchasedItem.transform, CameraLookAtTrans.transform, false);
        PlacingObject = false;
        PurchasedItem = null;
        Canvas.OpenShop();
    }

    #endregion


    #region Get Attributes
    public PlayerManager.Player GetPlayer()
    {
        return Player == null ? PlayerManager.Player.none : Player.GetPlayer();
    }
    #endregion
}
