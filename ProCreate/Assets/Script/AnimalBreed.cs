using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBreed : MonoBehaviour
{
    private AnimalStatistics CurrentAnimalStatistics;
    //private Breed breedscript;

    #region Built In Functions

    private void Awake()
    {
        CurrentAnimalStatistics = gameObject.GetComponent<AnimalStatistics>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region trigger enter/exit

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LivingPen"))
        {
            other.gameObject.GetComponent<PenList>().addAnimal(gameObject);
        }
        else if (other.gameObject.CompareTag("BreedingPen"))
        {
            PenList OtherPenList = other.gameObject.GetComponent<PenList>();
            if (CurrentAnimalStatistics.isWillingToBreed())
            {
                foreach (GameObject obj in OtherPenList.getAnimals())
                {
                    AnimalStatistics otherStastics = obj.GetComponent<AnimalStatistics>();
                    if(otherStastics.isWillingToBreed())
                    {
                        //breed and change breeding willingness

                        otherStastics.postBreedChange();
                        CurrentAnimalStatistics.postBreedChange();
                        break;
                    }
                }

            }
            OtherPenList.addAnimal(gameObject);

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
    #endregion


    #region spawn child

    #endregion
}
