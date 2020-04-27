using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfSource
{
    Water,
    Feed,
    None
}

public interface ResourceSource
{
    TypeOfSource GetTypeOfSource();

    float UnitsTakenFromSource();

    GameObject GetHeldObject();
}