using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    HoldingObject,
    HoldingAnimal,
    Empty
}

public class PlayerInteraction : MonoBehaviour
{
    #region Interaction Variables
    [Header("Interaction Variables")]
    [SerializeField] KeyCode GeneralInteractKey;

    [Header("Picking Up/Placing Variables")]
    [SerializeField] float ChildVerticalOffset;

    PlayerState CurrentState = PlayerState.Empty;
    GameObject ObjectInHand;

    #endregion

    #region Raycast Variables
    [Header("Raycast Variables")]
    [SerializeField] float RaycastLength = 2f;
    GameObject RaycastedObject;

    #endregion

    #region Built In Functions

    private void Update()
    {
        CheckForGeneralInteract();
    }

    #endregion

    #region Input

    void CheckForGeneralInteract()
    {
        if(Input.GetKeyDown(GeneralInteractKey))
        {
            switch(CurrentState)
            {
                case PlayerState.Empty:
                    InteractionRaycast();
                    break;
                case PlayerState.HoldingAnimal:
                    DropAnimal();
                    break;
                case PlayerState.HoldingObject:
                    TryToUseObject();
                    break;
            }

        }
    }

    #endregion

    #region Raycast Entities

    void InteractionRaycast()
    {

        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, transform.forward, out hit, RaycastLength, ~LayerMask.NameToLayer("Player")))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Animal>() != null)
                {
                    PickupAnimal(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.GetComponent<Interactable>() != null)
                {
                    PickupObject(hit.collider.gameObject);
                }

            }
        }

    }

    #endregion

    #region Pickup and Put Down Animals

    void PickupAnimal(GameObject animal)
    {
        Debug.Log("Picked up animal: " + animal.name);
        CurrentState = PlayerState.HoldingAnimal;
        Vector3 worldPos = animal.transform.position;
        Vector3 localPos = this.transform.InverseTransformPoint(worldPos);

        animal.GetComponent<AnimalMovement>().Pickedup();
        animal.transform.parent = this.gameObject.transform;
        animal.transform.localPosition = localPos + Vector3.up * ChildVerticalOffset;
        animal.layer = LayerMask.NameToLayer("Player");

        ObjectInHand = animal;
    }

    void DropAnimal()
    {
        Debug.Log("Dropped up animal: " + ObjectInHand.name);

        CurrentState = PlayerState.Empty;

        Vector3 localPos = ObjectInHand.transform.localPosition - Vector3.up * ChildVerticalOffset;
        Vector3 worldPos = this.transform.TransformPoint(localPos);

        ObjectInHand.layer = LayerMask.NameToLayer("Animal");

        ObjectInHand.GetComponent<AnimalMovement>().SetDown();

        ObjectInHand.transform.parent = null;
        ObjectInHand.transform.position = worldPos;
        //ObjectInHand.GetComponent<AnimalBreed>().attemptBreed();
        ObjectInHand = null;
    }


    #endregion

    #region Pickup and Use Objects

    void PickupObject(GameObject obj)
    {
        CurrentState = PlayerState.HoldingObject;
        Debug.Log("Picked up object: " + obj.GetComponent<Interactable>().GetTypeOfObject().ToString());
        ObjectInHand = obj;
    }

    void TryToUseObject()
    {
        if(RaycastedObject != null && ObjectInHand != null)
        {
            AnimalStatistics stats = RaycastedObject.GetComponent<AnimalStatistics>();
            Interactable interact = ObjectInHand.GetComponent<Interactable>();
            float food = 0;
            float water = 0;
            switch(interact.GetTypeOfObject())
            {
                case TypeOfObject.Water:
                    water = interact.UnitsTakenFromSource();
                    break;
                case TypeOfObject.BasicFeed:
                    food = interact.UnitsTakenFromSource();
                    break;
            }
            Debug.Log("Used up object: " + ObjectInHand.GetComponent<Interactable>().GetTypeOfObject().ToString());
            stats.GiveFoodAndWater(food, water);
        }
        CurrentState = PlayerState.Empty;
        ObjectInHand = null;
    }

    #endregion
}
