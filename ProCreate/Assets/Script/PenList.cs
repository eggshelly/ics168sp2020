using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenList : MonoBehaviour
{
    [Header("List of Animals")]
    [SerializeField] List<GameObject> animals = new List<GameObject>();

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
        Debug.Log("added : " + animal.name);
        animals.Add(animal);
    }
    public void removeAnimal(GameObject animal)
    {
        Debug.Log("removed : " + animal.name);
        animals.Remove(animal);
    }
    public void removePartner(GameObject other)
    {
        foreach(GameObject animal in animals){
            if(animal.GetComponent<AnimalBreed>().getPartner() == other)
            {
                animal.GetComponent<AnimalBreed>().setPartner(null);
            }
        }
    }

    public List<GameObject> getAnimals()
    {
        return animals;
    }
}
