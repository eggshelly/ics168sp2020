using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breed : MonoBehaviour
{


    [SerializeField] private GameObject other;

    #region Built In Function
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Notes
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
     *  AnimalCanvas
     * limb placement:
     * 
     * 
     * 
     */

    #endregion


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


        GameObject[] newAnimal = new GameObject[5];
        // body, head, legs, tail, canvas

        for(int i = 0;i < newAnimal.Length; i++)
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

        //apply color/material
        for (int i = 0; i < newAnimal.Length; i++)
        {
            // body, head, legs, tails, canvas

            GameObject newinstance = Instantiate(newAnimal[i]);
            newinstance.transform.parent = newchild.transform;
            if (i == 0)      
            {
                newbody = newinstance;
            }
            else if(i != 4)
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
            else if( i == 2 )
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
