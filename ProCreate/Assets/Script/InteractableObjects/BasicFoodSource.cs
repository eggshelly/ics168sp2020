using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFoodSource : MonoBehaviour, ResourceSource
{
    [Header("Interactable Attributes")]
    [SerializeField] TypeOfSource type = TypeOfSource.Feed;

    #region Variables unique to this object
    [Header("Object Attributes")]
    [SerializeField] bool UnlimitedSource = true;
    [SerializeField] float NumUnitsToTakeFromSource;
    [SerializeField] float MaxNumberOfUnits;

    [Header("Held Object For This Source")]
    [SerializeField] GameObject HeldHayPrefab;

    float CurrentNumberOfUnits;

    #endregion

    private void Start()
    {
        CurrentNumberOfUnits = MaxNumberOfUnits;
    }

    #region Functions for interacting with this object

    public TypeOfSource GetTypeOfSource()
    {
        return type;
    }

    public float UnitsTakenFromSource()
    {
        if (!UnlimitedSource)
        {
            if (CurrentNumberOfUnits > 0)
            {
                float units = (CurrentNumberOfUnits < NumUnitsToTakeFromSource ? CurrentNumberOfUnits : NumUnitsToTakeFromSource);
                CurrentNumberOfUnits -= units;
                return units;
            }
            return -1;
        }
        return NumUnitsToTakeFromSource;
    }

    public GameObject GetHeldObject()
    {
        return Instantiate(HeldHayPrefab);
    }

    #endregion

}
