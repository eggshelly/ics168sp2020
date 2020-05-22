using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfObject
{
    Bucket,
    Hay,
    ResourceCollector,
    None
}

public interface HeldObject
{
    TypeOfObject GetTypeOfObject();

    void ChangeObjectState();

    void PutOnGround();

    void SetNumUnitsHeld(float units = 0);

    float GetNumUnitsHeld();

    bool IsCarryingUnits();
}
