using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBreedState : MonoBehaviour
{


    private AnimalStatistics animalStatistics;


    [Header("Breeding partner")]
    GameObject partner = null;


    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LivingPen"))
        {
            other.gameObject.GetComponent<PenList>().addAnimal(gameObject);
        }
        else if (other.gameObject.CompareTag("BreedingPen"))
        {
            other.gameObject.GetComponent<PenList>().addAnimal(gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LivingPen"))
        {
            other.gameObject.GetComponent<PenList>().removeAnimal(gameObject);
        }
        else if (other.CompareTag("BreedingPen"))
        {
            other.gameObject.GetComponent<PenList>().removeAnimal(gameObject);

        }

    }
}
