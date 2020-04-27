using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket : MonoBehaviour, HeldObject
{
    [Header("Bucket Objects")]
    [SerializeField] GameObject EmptyBucket;
    [SerializeField] GameObject FilledBucket;

    [Header("Object Type")]
    [SerializeField] TypeOfObject objType = TypeOfObject.Bucket;

    bool isFilled = false;

    float GroundYPos;

    float NumUnits = 0;

    void Awake()
    {
        GroundYPos = this.transform.position.y;
    }

    #region Functions for Interacting with Object

    public void ChangeObjectState()
    {
        if(isFilled)
        {
            isFilled = false;
            EmptyBucket.SetActive(true);
            FilledBucket.SetActive(false);
        }
        else
        {
            isFilled = true;
            EmptyBucket.SetActive(false);
            FilledBucket.SetActive(true);
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

        if(units == 0)
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
