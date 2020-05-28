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

    Vector3 BoxSize;

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
        BoxSize = new Vector3(box.size.x, box.size.y / 2, box.size.z * 2f);
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

        Collider[] colls;

        Vector3 pos = this.transform.position + transform.forward * box.center.z;

        ExtDebug.DrawBox(pos + transform.forward * RaycastLength, BoxSize / 2, this.transform.rotation, Color.black);

        colls = Physics.OverlapBox(pos + transform.forward * RaycastLength, BoxSize / 2, this.transform.rotation, ~(1 << LayerMask.NameToLayer("Player")));

        if (colls.Length > 0) //Uhh this whole thing should run in O(n)? i think the way i set up the overlap box itself the max length <= 10? so shouldn't be too resource consuming
        {
            for(int i = 0; i < colls.Length; ++i)
            {
                if (colls[i].gameObject.GetComponent<Animal>() != null)
                {
                    PickupAnimal(colls[i].gameObject);
                    return;
                }
            }
           
           
            for (int i = 0; i < colls.Length; ++i)
            {
                if (colls[i].gameObject.GetComponent<Interactable>() != null)
                {
                    colls[i].gameObject.GetComponent<Interactable>().Interact();
                    return;
                }
            }
           
           
            for (int i = 0; i < colls.Length; ++i)
            {
                if (colls[i].gameObject.GetComponent<HeldObject>() != null)
                {
                    PickupObject(colls[i].gameObject);
                    return;
                }
            }
           
            for (int i = 0; i < colls.Length; ++i)
            {
                if (colls[i].gameObject.GetComponent<ResourceSource>() != null)
                {
                    CollectResource(colls[i].gameObject);
                    return;
                }
            }

        }

    }

    void RaycastForObjectUse()
    {
        RaycastHit hit;

        Vector3 pos = this.transform.position + transform.forward * box.center.z;

        if (Physics.BoxCast(pos, BoxSize / 2f, transform.forward, out hit, this.transform.rotation, (CurrentState != PlayerState.EmptyHand ? RaycastLength * 3f : RaycastLength), ~(1 << LayerMask.NameToLayer("Player"))))
        {
            if (hit.collider != null)
            {
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
        PlayerMove.HoldingObject(animal.GetComponent<BoxCollider>());

        CurrentState = PlayerState.HoldingAnimal;

        animal.GetComponent<AnimalMovement>().Pickedup();
        animal.transform.parent = this.gameObject.transform;
        animal.transform.localPosition = transform.InverseTransformPoint(this.transform.position + transform.forward * 0.6f);
        animal.layer = LayerMask.NameToLayer("Player");

        ObjectInHand = animal;

        RemoveAnimalFromPen();
    }

    void DropAnimal()
    {
        if (PlayerMove.IsFacingObstacle())
            return;
        else if (PlayerMove.PlayerInsidePen())
        {
            if (!AddAnimalToPen())
                return;
        }

        PlayerMove.PutDownObject();

        CurrentState = PlayerState.EmptyHand;

        Vector3 worldPos = this.transform.TransformPoint(ObjectInHand.transform.localPosition) + transform.forward * 0.2f;

        ObjectInHand.layer = LayerMask.NameToLayer("Animal");


        ObjectInHand.transform.parent = null;
        ObjectInHand.transform.position = worldPos;
        ObjectInHand.GetComponent<AnimalMovement>().SetDown();
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
                        break;
                    }
                    held.SetNumUnitsHeld(source.UnitsTakenFromSource());
                    held.ChangeObjectState();
                    FindObjectOfType<AudioManager>().Play("wellSFX");
                }
                break;
        }
    }

    void PickupObject(GameObject obj)
    {
        PlayerMove.HoldingObject(obj.GetComponent<BoxCollider>());


        CurrentState = PlayerState.HoldingObject;

        obj.transform.parent = this.transform;
        obj.transform.localPosition = transform.InverseTransformPoint(this.transform.position + transform.forward * 0.6f);
        obj.layer = LayerMask.NameToLayer("Player");

        ObjectInHand = obj;
    }

    void PutdownObject()
    {
        if (PlayerMove.IsFacingObstacle())
            return;

        PlayerMove.PutDownObject();
        if(ObjectInHand != null && CurrentState == PlayerState.HoldingObject)
        {
            Vector3 worldPos = this.transform.TransformPoint(ObjectInHand.transform.localPosition) + transform.forward * 0.2f;
            

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
            switch (obj.GetTypeOfObject())
            {
                case TypeOfObject.ResourceCollector:
                    stats.RetrieveResourceFromAnimal();
                    break;
                
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
        PlayerMove.PutDownObject();
    }

    void ConsumedPerpetualObject()
    {
        HeldObject obj = ObjectInHand.GetComponent<HeldObject>();
        obj.ChangeObjectState();
        obj.SetNumUnitsHeld();       
    }
    #endregion
}
