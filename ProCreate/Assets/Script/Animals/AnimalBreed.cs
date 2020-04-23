using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBreed : MonoBehaviour
{
    private AnimalStatistics CurrentAnimalStatistics;
    private AnimalMovement AnimalMovement;
    [SerializeField] GameObject partner;
    //private Breed breedscript;

    #region Built In Functions

    private void Awake()
    {
        CurrentAnimalStatistics = gameObject.GetComponent<AnimalStatistics>();
        AnimalMovement = gameObject.GetComponent<AnimalMovement>();
        partner = null;
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
            if (CurrentAnimalStatistics.isWillingToBreed() && partner==null)
            {
                Debug.Log("is willing LF> GF");
                foreach (GameObject obj in OtherPenList.getAnimals())
                {
                    AnimalStatistics otherStastics = obj.GetComponent<AnimalStatistics>();
                    AnimalBreed otherBreed = obj.GetComponent<AnimalBreed>();
                    if (otherStastics.isWillingToBreed() && otherBreed.getPartner() == null)
                    {
                        //breed and change breeding willingness
                        Debug.Log("SPAWNING BABY SOON");
                        partner = obj;
                        otherBreed.setPartner(gameObject);
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
            PenList penList = other.gameObject.GetComponent<PenList>();
            penList.removeAnimal(gameObject);
            penList.removePartner(gameObject);
            partner = null;
        }

    }
    #endregion


    #region spawn child
    public void attemptBreed()
    {
        if (partner)
        {
            spawnChild(partner);
            partner.GetComponent<AnimalStatistics>().postBreedChange();
            CurrentAnimalStatistics.postBreedChange();
            //maybe will get rid of this
            partner.GetComponent<AnimalBreed>().setPartner(null);
            partner = null;
        }
    }

    public void spawnChild(GameObject other)//im sorry this code is messy i'll refactor
    {
        Material main;
        Material second;

        // get main color/ material from head
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            main = this.gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[0];
        else
            main = other.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[0];
        //get secondary color/material from head
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            second = this.gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[1];
        else
            second = other.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sharedMaterials[1];


        GameObject[] newAnimal = new GameObject[5];
        // body, head, legs, tail, canvas

        for (int i = 0; i < newAnimal.Length; i++)
        {
            if (Random.Range(0.0f, 1.0f) <= 0.5f)
                newAnimal[i] = this.gameObject.transform.GetChild(i).gameObject;
            else
                newAnimal[i] = other.transform.GetChild(i).gameObject;
        }


        GameObject newchild = (Random.Range(0.0f, 1.0f) <= 0.5f ? Instantiate(gameObject) : Instantiate(other));
        newchild.transform.position = Vector3.zero;
        newchild.transform.rotation = Quaternion.identity;
        newchild.transform.localScale = Vector3.one;
        foreach (Transform child in newchild.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject newbody = null;

        //apply color/material/pivot
        for (int i = 0; i < newAnimal.Length; i++)
        {
            // body, head, legs, tails, canvas

            GameObject newinstance = Instantiate(newAnimal[i]);
            newinstance.transform.parent = newchild.transform;
            if (i == 0)
            {
                newbody = newinstance;
            }
            else if (i != 4)
            {   // apply head/tail/leg pivots
                newinstance.transform.position = newbody.transform.GetChild(i - 1).position;
            }

            if (i != 2 && i != 4)   //head and legs
            {
                MeshRenderer temp = newinstance.GetComponent<MeshRenderer>();
                Material[] tempMat = temp.sharedMaterials;
                tempMat[0] = main;
                if (tempMat.Length == 2)
                {
                    tempMat[1] = second;
                }
                temp.materials = tempMat;
            }
            else if (i == 2)
            {   //specifically for legs
                for (int j = 0; j < newinstance.transform.childCount; j++)
                {
                    MeshRenderer temp = newinstance.transform.GetChild(j).GetComponent<MeshRenderer>();
                    Material[] tempMat = temp.sharedMaterials;
                    tempMat[0] = main;
                    if (tempMat.Length == 2)
                    {
                        tempMat[1] = second;
                    }
                    temp.materials = tempMat;
                }

            }
        }

        //set child between to parent
        newchild.transform.position = (gameObject.transform.position + other.transform.position) / 2 + Vector3.up * 2;


    }
    #endregion




    #region getter/setter
    public GameObject getPartner()
    {
        return partner;
    }
    public void setPartner(GameObject other)
    {
        partner = other;
    }
    #endregion
}
