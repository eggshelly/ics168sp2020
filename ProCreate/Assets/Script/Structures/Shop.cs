using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, Interactable
{


    #region Variables for Player in Shop

    bool IsOccupied = false;

    PlayerMovement Player;

    #endregion

    #region Components

    BoxCollider box;

    #endregion 

    private void Awake()
    {
        box = this.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(IsOccupied)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();
            }
        }
    }


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
                Debug.Log("Hey there " + Player.gameObject.name);
                Player.UsingShop(true);
            }
        }
    }

    void CloseShop()
    {
        Player.UsingShop(false);
        Player = null;
        IsOccupied = false;
    }

}
