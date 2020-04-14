using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breed : MonoBehaviour
{

    [SerializeField] private GameObject other;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     * Michelle's notes for breeding/spawn:
     * 
     * parent scheme:
     * Main obj(pivot point)
     *  body
     *      head_attach (pivot)
     *      leg_attach (pivot)      these piviot points will match with the responding components (same coordinates)
     *      tail_attach (pivot)
     *  head    
     *  legs
     *      B_L
     *      B_R
     *      F_L
     *      F_R
     *  tail
     * limb placement:
     * 
     * 
     * 
     */

        //TODO: reorder children to accomodate for new grandparent scheme


 
    public void spawnChild(/*GameObject other*/)
    {
        //TODO REPLACE otherParent with other, using other parent is for debuggin
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


        GameObject[] newAnimal = new GameObject[4];
        // body, head, legs, tail

        //get body
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            newAnimal[0] = this.gameObject.transform.GetChild(0).gameObject;
        else
            newAnimal[0] = other.transform.GetChild(0).gameObject;

        //get head
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            newAnimal[1] = this.gameObject.transform.GetChild(1).gameObject;
        else
            newAnimal[1] = other.transform.GetChild(1).gameObject;

        //get tail
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            newAnimal[3] = this.gameObject.transform.GetChild(3).gameObject;
        else
            newAnimal[3] = other.transform.GetChild(3).gameObject;


        
        //get limbs
        if (Random.Range(0.0f, 1.0f) <= 0.5f)
            newAnimal[2] = this.gameObject.transform.GetChild(2).gameObject;
        else
            newAnimal[2] = other.transform.GetChild(2).gameObject;

        

        GameObject newchild = new GameObject("newHybrid");

        GameObject newbody = null;

        //apply color/material
        for (int i = 0; i < newAnimal.Length; i++)
        {
            // body, head, legs, tailS
            //need to map head, leg, tail to body piviot point

            GameObject newinstance = Instantiate(newAnimal[i]);
            newinstance.transform.parent = newchild.transform;
            if (i == 0)      
            {
                newbody = newinstance;
            }
            else
            {   // apply head/tail/leg pivots
                newinstance.transform.position = newbody.transform.GetChild(i - 1).position;
            }

            if (i != 2)
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
            else
            {   //specifically for legs
                for(int j = 0; j < newinstance.transform.childCount; j++)
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
  








    }
}
