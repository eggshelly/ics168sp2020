using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    left,
    right,
    forward,
    backwards,
    f_right,
    f_left,
    b_right,
    b_left,
    neutral
}


public class PlayerMovement : MonoBehaviour
{
    #region player defination

    [SerializeField] PlayerManager.Player player;
    #endregion

    #region Movement Variables
    [Header("Movement Settings")]
    [SerializeField] float SpeedMultiplier = 10f;
    [SerializeField] Directions FacingDirection = Directions.forward;

    Vector2 ObjectInDirection = new Vector2();

    bool isHoldingObject;
    BoxCollider ObjBox;

    #endregion

    #region Raycast Variables
    [SerializeField] float RaycastLength = 0.5f;


    Vector3 BoxCenter;
    Vector3 BoxSize;


    #endregion

    #region Detecting Player Inside Structure Variables
    [Header("Debugging Entering Pen")] // comment this header out if not debugging
    [SerializeField] bool IsInsidePen= false;
    [SerializeField] GameObject PenStructure;

    bool IsInShop = false;

    #endregion

    #region Components

    BoxCollider box;

    #endregion

    private void Awake()
    {
        box = this.GetComponent<BoxCollider>();
        BoxCenter = box.center;
        BoxSize = new Vector3(box.size.x * 0.8f, box.size.y / 2, box.size.z);
        Physics.queriesHitBackfaces = true;
    }

    private void FixedUpdate()
    {
        if (IsInShop)
            return;

        Vector2 directions = ChangePlayerDirection();
        GeneralRaycast();
        MovePlayer(directions);
    }


    #region Movement


    //Moves the transform of the player 
    void MovePlayer(Vector2 directions)
    {
        Vector3 posOffset = (Vector3.right * directions.x * ObjectInDirection.x + Vector3.forward * directions.y * ObjectInDirection.y);
        this.transform.position += posOffset * Time.deltaTime * SpeedMultiplier;
    }

    //Updates the Directions variable holding the direction the player is facing
    Vector2 ChangePlayerDirection()
    {

        float horizontal = 0;
        float vertical = 0;
        InputManager.processMovementInput(player, ref horizontal,ref vertical);

        if (horizontal < 0)
        {
            if (vertical < 0)
            {
                FacingDirection = Directions.b_left;
                this.transform.rotation = Quaternion.Euler(0, -135, 0);
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.f_left;
                this.transform.rotation = Quaternion.Euler(0, -45, 0);
            }
            else
            {
                FacingDirection = Directions.left;
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
        else if (horizontal > 0)
        {
            if (vertical < 0)
            {
                FacingDirection = Directions.b_right;
                this.transform.rotation = Quaternion.Euler(0, 135, 0);
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.f_right;
                this.transform.rotation = Quaternion.Euler(0, 45, 0);
            }
            else
            {
                FacingDirection = Directions.right;
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
        else
        {
            if (vertical < 0)
            {
                FacingDirection = Directions.backwards;
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.forward;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        return new Vector2(horizontal, vertical);
    }
    #endregion

    #region Raycast Entities

    void UpdateObjectInDirection(GameObject obstacle = null)
    {
        if(obstacle == null)
        {
            ObjectInDirection = Vector2.one;
            return;
        }


        bool HitObject = false;

        switch (FacingDirection)
        {
            case Directions.backwards:
                ObjectInDirection.y = 0;
                break;
            case Directions.forward:
                ObjectInDirection.y = 0;
                break;
            case Directions.left:
                ObjectInDirection.x = 0;
                break;
            case Directions.right:
                ObjectInDirection.x = 0;
                break;
            case Directions.b_left:
                if (DirectionalRaycast(Vector3.right * -1))
                {
                    HitObject = true;
                    ObjectInDirection.x = 0;
                }
                if (DirectionalRaycast(Vector3.back))
                {
                    HitObject = true;
                    ObjectInDirection.y = 0;
                }

                if (!HitObject)
                {
                    if (DirectionalRaycast(transform.forward))
                    {
                        ObjectInDirection = Vector2.zero;
                    }
                }

                break;
            case Directions.b_right:
                if (DirectionalRaycast(Vector3.right))
                {
                    HitObject = true;
                    ObjectInDirection.x = 0;
                }
                if (DirectionalRaycast(Vector3.back))
                {
                    HitObject = true;
                    ObjectInDirection.y = 0;
                }

                if (!HitObject)
                {
                    if (DirectionalRaycast(transform.forward))
                    {
                        ObjectInDirection = Vector2.zero;
                    }
                }

                break;
            case Directions.f_left:
                if (DirectionalRaycast(Vector3.right * -1))
                {
                    HitObject = true;
                    ObjectInDirection.x = 0;
                }
                if (DirectionalRaycast(Vector3.forward))
                {
                    HitObject = true;
                    ObjectInDirection.y = 0;
                }

                if (!HitObject)
                {
                    if (DirectionalRaycast(transform.forward))
                    {
                        ObjectInDirection = Vector2.zero;
                    }
                }
                break;
            case Directions.f_right:
                if (DirectionalRaycast(Vector3.right))
                {
                    HitObject = true;
                    ObjectInDirection.x = 0;
                }
                if (DirectionalRaycast(Vector3.forward))
                {
                    HitObject = true;
                    ObjectInDirection.y = 0;
                }

                if (!HitObject)
                {
                    if (DirectionalRaycast(transform.forward))
                    {
                        ObjectInDirection = Vector2.zero;
                    }
                }

                break;
        }

    }

    bool DirectionalRaycast(Vector3 dir, bool isDiagonal = false)
    {
        RaycastHit hit;

        Vector3 pos = GetPosToRaycastFrom();


        float longerRay = 2 * RaycastLength;

        Debug.DrawRay(pos, dir * RaycastLength, Color.red, 3f);
        if (Physics.Raycast(pos, dir, out hit, (isDiagonal ? Mathf.Sqrt(2 * Mathf.Pow(longerRay, 2)) : longerRay), ~(1 << LayerMask.NameToLayer("Player"))))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    void GeneralRaycast()
    {
        Collider[] colls;
        Crossable cross;
        Pen pen;

        Vector3 pos = GetPosToBoxCastFrom();
        Vector3 size = GetSizeOfBox(pos);

        colls = Physics.OverlapBox(pos + transform.forward * RaycastLength, size, this.transform.rotation, ~(1 << LayerMask.NameToLayer("Player")));

        ExtDebug.DrawBox(pos + transform.forward * RaycastLength, size, this.transform.rotation, Color.black);



        CheckIfEnterOrLeftPen(colls);


        if (colls.Length > 0)
        {
            if(colls.Length == 1)
            {
                cross = colls[0].gameObject.GetComponent<Crossable>();
                if ((cross != null && cross.IsOpen()))
                {
                    Debug.Log("Hit only the gate");
                    UpdateObjectInDirection();
                    return;
                }

                pen = colls[0].gameObject.GetComponent<Pen>();
                if (pen != null)
                {
                    Debug.Log("Hit only the pen");
                    UpdateObjectInDirection();
                    return;
                }


                UpdateObjectInDirection(colls[0].gameObject);

            }
            else if(colls.Length == 2)
            {
                cross = colls[0].gameObject.GetComponent<Crossable>();
                pen = colls[1].gameObject.GetComponent<Pen>();

                if((cross != null && cross.IsOpen()) && pen != null)
                {
                    UpdateObjectInDirection();
                    return;
                }

                cross = colls[1].gameObject.GetComponent<Crossable>();
                pen = colls[0].gameObject.GetComponent<Pen>();

                if ((cross != null && cross.IsOpen()) && pen != null)
                {
                    UpdateObjectInDirection();
                    return;
                }

                UpdateObjectInDirection(colls[0].gameObject);

            }
            else
            {

                for(int i = 0; i < colls.Length; ++i)
                {
                    cross = colls[0].gameObject.GetComponent<Crossable>();
                    pen = colls[0].gameObject.GetComponent<Pen>();
                    if (cross == null && pen == null)
                    {
                        UpdateObjectInDirection(colls[i].gameObject);
                        return;
                    }
                }
            }

            //Checks if the object it collided with is something that can be crossed or if the player is inside the pen



        }
        else
        {
            UpdateObjectInDirection();
        }
    }
    void CheckIfEnterOrLeftPen(Collider[] colls)
    {
        for(int i = 0; i < colls.Length; ++i)
        {
            Pen pen = colls[i].GetComponent<Pen>();
            if (pen != null)
            {
                IsInsidePen = true;
                PenStructure = pen.GetPenStructure();
                return;
            }
        }
        IsInsidePen = false;
        PenStructure = null;

    }

    Vector3 GetPosToBoxCastFrom()
    {
        Vector3 pos;
        if (isHoldingObject)
        {
            Vector3 ObjPos = this.transform.TransformPoint(ObjBox.gameObject.transform.localPosition + ObjBox.center);
            ObjPos.y = this.transform.position.y;
            pos = ((this.transform.position + transform.forward * BoxCenter.z) + ObjPos) / 2f;
        }
        else
        {
            pos = this.transform.position + transform.forward * BoxCenter.z;
        }


        return pos;
    }

    Vector3 GetPosToRaycastFrom()
    {
        Vector3 pos;
        if(isHoldingObject)
        {
            pos = this.transform.TransformPoint(ObjBox.gameObject.transform.localPosition + ObjBox.center);
            pos.y = this.transform.position.y;
        }
        else
        {
            pos = this.transform.position + transform.forward * BoxCenter.z;

        }
        return pos;
    }

    Vector3 GetSizeOfBox(Vector3 pos)
    {
        Vector3 size;
        if (isHoldingObject)
        {
            float x = Mathf.Max(BoxSize.x, ObjBox.size.x);
            float y = Mathf.Max(BoxSize.y, ObjBox.size.y);
            float z = Mathf.Max(BoxSize.z + (pos - (this.transform.position + BoxCenter)).magnitude, ObjBox.size.z + (pos - (ObjBox.transform.position + ObjBox.center)).magnitude);

            size = new Vector3(x, y, z) / 2;

        }
        else
        {
            size = BoxSize / 2;
        }


        return size;
    }



    #endregion

    #region Update Object In Hand

    public void HoldingObject(BoxCollider box)
    {
        isHoldingObject = true;
        ObjBox = box;
    }

    public void PutDownObject()
    {
        isHoldingObject = false;
        ObjBox = null;
    }

    #endregion

    #region Get Attributes

    public bool PlayerInsidePen()
    {
        return IsInsidePen;
    }

    public GameObject GetPenStructure()
    {
        return PenStructure;
    }
    public PlayerManager.Player GetPlayer()
    {
        return player;
    }

    #endregion

    #region Shop

    public void UsingShop(bool b)
    {
        IsInShop = b;
    }

    public bool IsFacingObstacle()
    {
        return ObjectInDirection != Vector2.one;
    }

    #endregion


}
