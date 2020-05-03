using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    HoldingObject,
    HoldingAnimal,
    EmptyHand
}

public class PlayerInteraction : MonoBehaviour
{
    #region Interaction Variables
    [Header("Interaction Variables")]
    [SerializeField] KeyCode GeneralInteractKey;

    [Header("Picking Up/Placing Variables")]
    [SerializeField] float ChildVerticalOffset = 0.5f;

    PlayerState CurrentState = PlayerState.EmptyHand;
    GameObject ObjectInHand;

    #endregion

    #region Raycast Variables
    [Header("Raycast Variables")]
    [SerializeField] float RaycastLength = 1f;
    GameObject RaycastedObject;

    #endregion

    #region Needed Components
    BoxCollider box;
    PlayerMovement PlayerMove;

    #endregion

    #region Built In Functions

    private void Awake()
    {
        box = this.GetComponent<BoxCollider>();
        PlayerMove = this.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForGeneralInteract();
    }

    #endregion

    #region Input

    void CheckForGeneralInteract()
    {
        if(InputManager.interact(PlayerMove.GetPlayer()))
        {
            switch(CurrentState)
            {
                case PlayerState.EmptyHand:
                    InteractionRaycast();
                    break;
                case PlayerState.HoldingAnimal:
                    DropAnimal();
                    break;
                case PlayerState.HoldingObject:
                    RaycastForObjectUse();
                    break;
            }

        }
    }

    #endregion

    #region Raycast Entities

    void InteractionRaycast()
    {

        RaycastHit hit;

        Vector3 pos = this.transform.position + transform.forward * box.center.z;

        if (Physics.BoxCast(pos, box.bounds.extents, transform.forward, out hit, this.transform.rotation, RaycastLength, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            if (hit.collider != null)
            {
                Debug.Log("Not null");
                if (hit.collider.gameObject.GetComponent<Animal>() != null)
                {
                    PickupAnimal(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.GetComponent<ResourceSource>() != null)
                {
                    CollectResource(hit.collider.gameObject);
                }
                else if(hit.collider.gameObject.GetComponent<Interactable>() != null)
                {
                    Debug.Log("Here");
                    hit.collider.gameObject.GetComponent<Interactable>().Interact();
                }
                else if(hit.collider.gameObject.GetComponent<HeldObject>() != null)
                {
                    PickupObject(hit.collider.gameObject);
                }

            }
        }

    }

    void RaycastForObjectUse()
    {
        RaycastHit hit;

        Vector3 pos = this.transform.position + transform.forward * box.center.z;

        if (Physics.BoxCast(pos, box.bounds.extents, transform.forward, out hit, this.transform.rotation, RaycastLength, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name + " " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                if (hit.collider.gameObject.GetComponent<Animal>() != null)
                {
                    TryToUseObject(hit.collider.gameObject);
                    return;
                }
                else if (hit.collider.gameObject.GetComponent<ResourceSource>() != null)
                {
                    CollectResource(hit.collider.gameObject);
                    return;
                }
            }
        }
        PutdownObject();
    }

    #endregion

    #region Pickup and Put Down Animals

    void PickupAnimal(GameObject animal)
    {
        CurrentState = PlayerState.HoldingAnimal;
        Vector3 worldPos = animal.transform.position;
        Vector3 localPos = this.transform.InverseTransformPoint(worldPos);

        animal.GetComponent<AnimalMovement>().Pickedup();
        animal.transform.parent = this.gameObject.transform;
        animal.transform.localPosition = localPos + Vector3.up * ChildVerticalOffset;
        animal.layer = LayerMask.NameToLayer("Player");

        ObjectInHand = animal;

        RemoveAnimalFromPen();
    }

    void DropAnimal()
    {
        if(PlayerMove.PlayerInsidePen())
        {
            if (!AddAnimalToPen())
                return;
        }

        CurrentState = PlayerState.EmptyHand;

        Vector3 localPos = ObjectInHand.transform.localPosition - Vector3.up * ChildVerticalOffset;
        Vector3 worldPos = this.transform.TransformPoint(localPos);

        ObjectInHand.layer = LayerMask.NameToLayer("Animal");

        ObjectInHand.GetComponent<AnimalMovement>().SetDown();

        ObjectInHand.transform.parent = null;
        ObjectInHand.transform.position = worldPos;
        ObjectInHand = null;
    }

    bool AddAnimalToPen()
    {
        Pen pen = PlayerMove.GetPenStructure().GetComponent<Pen>();
        if(pen.HasRoomForAnimal())
        {
            pen.AddAnimalToPen(ObjectInHand);
            return true;
        }
        return false;

    }

    void RemoveAnimalFromPen()
    {
        if(PlayerMove.PlayerInsidePen())
        {
            Pen pen = PlayerMove.GetPenStructure().GetComponent<Pen>();
            pen.RemoveAnimalFromPen(ObjectInHand);
        }
    }

    #endregion

    #region Pickup and Use Objects

    void CollectResource(GameObject obj)
    {
        ResourceSource source = obj.GetComponent<ResourceSource>();

        switch(CurrentState)
        {
            case PlayerState.EmptyHand:
                if(source.GetTypeOfSource() == TypeOfSource.Feed)
                {
                    GameObject held = source.GetHeldObject();
                    if (held== null)
                        break;

                    held.GetComponent<HeldObject>().SetNumUnitsHeld(source.UnitsTakenFromSource());
                    PickupObject(held);
                }
                break;
            case PlayerState.HoldingObject:
                if(source.GetTypeOfSource() == TypeOfSource.Water)
                {
                    HeldObject held = ObjectInHand.GetComponent<HeldObject>();
                    if (held.GetTypeOfObject() != TypeOfObject.Bucket || held.IsCarryingUnits())
                    {
                        Debug.Log("Breaking");
                        break;
                    }
                    held.SetNumUnitsHeld(source.UnitsTakenFromSource());
                    Debug.Log("Changing state?");
                    held.ChangeObjectState();
                }
                break;
        }
    }

    void PickupObject(GameObject obj)
    {
        CurrentState = PlayerState.HoldingObject;

        obj.transform.parent = this.transform;
        obj.transform.localPosition = transform.InverseTransformPoint(this.transform.position + transform.forward * 0.6f);
        obj.layer = LayerMask.NameToLayer("Player");

        ObjectInHand = obj;
    }

    void PutdownObject()
    {
        if(ObjectInHand != null && CurrentState == PlayerState.HoldingObject)
        {
            Vector3 worldPos = this.transform.TransformPoint(ObjectInHand.transform.localPosition) + transform.forward * 0.4f;
            

            ObjectInHand.transform.parent = null;
            ObjectInHand.transform.position = worldPos;
            ObjectInHand.GetComponent<HeldObject>().PutOnGround();

            ObjectInHand.layer = LayerMask.NameToLayer("Obstacle");

            ObjectInHand = null;
            CurrentState = PlayerState.EmptyHand;

        }
    }

    void TryToUseObject(GameObject animal)
    {
        if(ObjectInHand != null)
        {
            AnimalStatistics stats = animal.GetComponent<AnimalStatistics>();
            HeldObject obj = ObjectInHand.GetComponent<HeldObject>();

            if (!obj.IsCarryingUnits())
                return;

            float food = 0;
            float water = 0;
            switch(obj.GetTypeOfObject())
            {
                case TypeOfObject.Bucket:                    
                    water = (obj.IsCarryingUnits() ? obj.GetNumUnitsHeld() : 0);
                    ConsumedPerpetualObject();
                    break;
                case TypeOfObject.Hay:
                    food = obj.GetNumUnitsHeld();
                    ConsumedOneTimeUseObject();
                    break;
            }
            stats.GiveFoodAndWater(food, water);
        }
    }

    void ConsumedOneTimeUseObject()
    {
        Destroy(ObjectInHand);
        CurrentState = PlayerState.EmptyHand;
        ObjectInHand = null;
    }

    void ConsumedPerpetualObject()
    {
        HeldObject obj = ObjectInHand.GetComponent<HeldObject>();
        obj.ChangeObjectState();
        obj.SetNumUnitsHeld();       
    }
    #endregion
}
