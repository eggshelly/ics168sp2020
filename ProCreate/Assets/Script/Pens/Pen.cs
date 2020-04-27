using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Pen
{
    GameObject GetPenStructure();

    void AddAnimalToPen(GameObject animal);

    void RemoveAnimalFromPen(GameObject animal);

    bool HasRoomForAnimal();
}
