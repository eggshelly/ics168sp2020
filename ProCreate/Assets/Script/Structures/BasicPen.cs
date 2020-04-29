using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPen : MonoBehaviour, Pen
{
    #region Attributes

    [Header("Pen Attributes")]
    [SerializeField] protected int capacity;

    [SerializeField] protected int CurrentCapacity = 0;

    #endregion

    #region Pen Contents

    [SerializeField] protected List<GameObject> AnimalsInPen;

    #endregion

    #region Built In Functions

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    #endregion


    #region Changing the Animals in the Pen

    public void AddAnimalToPen(GameObject animal)
    {
        AnimalsInPen.Add(animal);
        CurrentCapacity += 1;
    }

    public void RemoveAnimalFromPen(GameObject animal)
    {
        if (AnimalsInPen.Contains(animal))
        {
            AnimalsInPen.Remove(animal);
            CurrentCapacity -= 1;
        }
    }
    
    public bool HasRoomForAnimal()
    {
        return CurrentCapacity < capacity;
    }

    #endregion

    #region Get Attribues/Objects

    public GameObject GetPenStructure()
    {
        return this.gameObject;
    }
    #endregion
}
