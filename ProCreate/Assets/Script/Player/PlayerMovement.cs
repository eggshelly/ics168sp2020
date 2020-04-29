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
    #region Movement Variables
    [Header("Movement Settings")]
    [SerializeField] float SpeedMultiplier = 10f;
    [SerializeField] Directions FacingDirection = Directions.forward;

    Vector2 ObjectInDirection = new Vector2();

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
        this.transform.position += (Vector3.right * directions.x * ObjectInDirection.x + Vector3.forward * directions.y * ObjectInDirection.y) * Time.deltaTime * SpeedMultiplier;
    }

    //Updates the Directions variable holding the direction the player is facing
    Vector2 ChangePlayerDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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

        Vector3 pos = this.transform.position + transform.forward * (BoxCenter.z + BoxSize.z / 2);


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
        RaycastHit hit;
        Collider[] colls;
        Collider coll;

        Vector3 pos = this.transform.position + transform.forward * BoxCenter.z ;

        ExtDebug.DrawBoxCastOnHit(pos, BoxSize / 2, this.transform.rotation, transform.forward, RaycastLength, Color.red);

        bool hitByBoxCast = Physics.BoxCast(pos, BoxSize / 2, transform.forward, out hit, this.transform.rotation, RaycastLength, ~(1 << LayerMask.NameToLayer("Player")));
        colls = Physics.OverlapBox(pos + transform.forward * BoxSize.z / 2, BoxSize / 2, this.transform.rotation, ~(1 << LayerMask.NameToLayer("Player")));

        CheckIfEnterOrLeftPen(colls);


        if (hitByBoxCast || colls.Length > 0)
        {
            coll = (hit.collider != null ? hit.collider : colls[0]);


            //Checks if the object it collided with is something that can be crossed or if the player is inside the pen
            Crossable cross = coll.gameObject.GetComponent<Crossable>();
            Pen pen = coll.gameObject.GetComponent<Pen>();


            if ((cross != null && cross.IsOpen()) || pen != null)
            {
                UpdateObjectInDirection();
            }
            else
            {
                UpdateObjectInDirection(coll.gameObject);
            }


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

    #endregion

    #region Shop

    public void UsingShop(bool b)
    {
        IsInShop = b;
    }

    #endregion


}
