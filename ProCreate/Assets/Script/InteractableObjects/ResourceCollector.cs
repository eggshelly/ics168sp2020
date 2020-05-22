using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour, HeldObject
{
    [Header("Object Type")]
    [SerializeField] TypeOfObject objType = TypeOfObject.ResourceCollector;

    bool isFilled = true;

    float GroundYPos;

    float NumUnits = 0;

    private void Awake()
    {
        GroundYPos = this.transform.position.y;
    }
    #region Functions for Interacting with Object
    public void ChangeObjectState()
    {
        if (isFilled)
        {
            isFilled = false;
        }
        else
        {
            isFilled = true;
        }
    }
    public void PutOnGround()
    {
        this.transform.position = new Vector3(this.transform.position.x, GroundYPos, this.transform.position.z);
    }
    #endregion

    #region Setting/Getting Contents of Object
    public void SetNumUnitsHeld(float units = 0)
    {
        NumUnits = units;

        if (units == 0)
        {
            isFilled = false;
        }
    }

    public float GetNumUnitsHeld()
    {
        return NumUnits;
    }
    #endregion

    #region GetFunctions
    public TypeOfObject GetTypeOfObject()
    {
        return objType;
    }

    public bool IsCarryingUnits()
    {
        return isFilled;
    }
    #endregion






}
