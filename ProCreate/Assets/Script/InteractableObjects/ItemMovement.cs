using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] float YOffset = 0.5f;
    [SerializeField] float DistanceToMove = 1f;

    float GroundYPos;
    LayerMask OriginalLayer;


    Shop Shop;



    bool Moving = false;
    bool Placing = false;
    bool CanPlace = false;

    BoxCollider coll;

    private void Awake()
    {
        GroundYPos = this.transform.position.y;
        coll = this.GetComponent<BoxCollider>();
        DetectLayer();
    }

    void DetectLayer()
    {
        OriginalLayer = this.gameObject.layer;

        if(OriginalLayer == LayerMask.NameToLayer("Animal"))
        {
            this.GetComponent<AnimalMovement>().Pickedup();
        }
        
        RecursivelySetLayer(this.gameObject.transform, LayerMask.NameToLayer("CurrentStructure"));
    }

    void RecursivelySetLayer(Transform trans, LayerMask newLayer)
    {
        trans.gameObject.layer = newLayer;

        foreach(Transform t in trans.transform)
        {
            RecursivelySetLayer(t, newLayer);
        }
    }


    public void Purchased(Vector3 playerPos, Shop s)
    {
        this.transform.position = playerPos + Vector3.up * YOffset;
        this.Shop = s;
    }

    private void Update()
    {
        if(InputManager.interact(Shop.GetPlayer()))
        {
            if(!Placing && CanPlace)
            {
                PlaceObject();
            }
        }
        else if (InputManager.cancel(Shop.GetPlayer()))
        {
            Shop.CancelPurchase();
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        MoveObjectPosition();
        CheckForObstruction();
    }

    void MoveObjectPosition()
    {
        float horizontal = 0;//= Input.GetAxisRaw("Horizontal");
        float vertical = 0;//= Input.GetAxisRaw("Vertical");

        InputManager.processMovementInput(Shop.GetPlayer(), ref horizontal, ref vertical);

        if (!Moving)
        {
            StartCoroutine(MoveObj(horizontal, vertical));
        }
    }

    void CheckForObstruction()
    {
        Collider[] colls;

        ExtDebug.DrawBoxCastOnHit(this.transform.position, coll.bounds.extents * 1.3f, this.transform.rotation, transform.forward, 0, Color.red);

        colls = Physics.OverlapBox(this.transform.position, coll.bounds.extents * 1.3f, this.transform.rotation, ~(1 << this.gameObject.layer));

        if(colls.Length > 0)
        {
            CanPlace = false;
        }
        else
        {
            CanPlace = true;
        }
        //Debug.Log("Can Place: " + CanPlace);
    }

    IEnumerator MoveObj(float hor, float vert)
    {
        Moving = true;
        
        this.transform.position += (Vector3.right * hor + Vector3.forward * vert) * DistanceToMove;

        yield return new WaitForSeconds(0.05f);
       
        Moving = false;
    }
    
    void  PlaceObject()
    {
        this.transform.position = new Vector3(this.transform.position.x, GroundYPos, this.transform.position.z);

        Shop.FinalizePurchase();

        RecursivelySetLayer(this.transform, OriginalLayer);

        if (OriginalLayer == LayerMask.NameToLayer("Animal"))
        {
            this.GetComponent<AnimalMovement>().SetDown();
        }

        Destroy(this);
    }


}
