using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfObject
{
    Water,
    BasicFeed,
    None
}

public interface Interactable
{
    TypeOfObject GetTypeOfObject();

    float UnitsTakenFromSource();
}