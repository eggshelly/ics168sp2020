using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfObject
{
    Water,
    AnimalFeed,
    None
}

public interface Interactable
{
    TypeOfObject GetTypeOfObject();

    float UnitsTakenFromSource();
}