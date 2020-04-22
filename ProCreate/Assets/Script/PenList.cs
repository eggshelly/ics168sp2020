using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenList : MonoBehaviour
{
    [Header("List of Animals")]
    List<GameObject> animals = new List<GameObject>();

    #region Built In Functions
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void addAnimal(GameObject animal)
    {
        animals.Add(animal);
    }
    public void removeAnimal(GameObject animal)
    {
        animals.Remove(animal);
    }
}
