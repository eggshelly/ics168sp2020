using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Directions
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
    [SerializeField] float SpeedMultiplier = 0.25f;
    [SerializeField] Directions FacingDirection = Directions.neutral;

    #endregion

    #region Raycast Variables
    [SerializeField] float RaycastLength = 2f;

    GameObject RaycastedObject;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RaycastForObjects();
    }

    #region Movement


    //Moves the transform of the player 
    void MovePlayer()
    {
        Vector2 directions = ChangePlayerDirection();

        this.transform.Translate((Vector3.right * directions.x + Vector3.forward * directions.y) * Time.deltaTime * SpeedMultiplier);
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
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.f_left;
            }
            else
            {
                FacingDirection = Directions.left;
            }
        }
        else if (horizontal > 0)
        {
            if (vertical < 0)
            {
                FacingDirection = Directions.b_right;
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.f_right;
            }
            else
            {
                FacingDirection = Directions.right;
            }
        }
        else
        {
            if (vertical < 0)
            {
                FacingDirection = Directions.backwards;
            }
            else if (vertical > 0)
            {
                FacingDirection = Directions.forward;
            }
        }

        return new Vector2(horizontal, vertical);
    }
    #endregion

    #region Raycast Entities

    //Raycasts in the direction the player is looking to check for  
    void RaycastForObjects()
    {
        RaycastHit hit;

        Vector3 CastDirection = new Vector3();

        switch(FacingDirection)
        {
            case Directions.backwards:
                CastDirection.x = 0;
                CastDirection.z = -1;
                break;
            case Directions.forward:
                CastDirection.x = 0;
                CastDirection.z = 1;
                break;
            case Directions.left:
                CastDirection.x = -1;
                CastDirection.z = 0;
                break;
            case Directions.right:
                CastDirection.x = 1;
                CastDirection.z = 0;
                break;
            case Directions.b_left:
                CastDirection.x = -1;
                CastDirection.z = -1;
                break;
            case Directions.b_right:
                CastDirection.x = 1;
                CastDirection.z = -1;
                break;
            case Directions.f_left:
                CastDirection.x = -1;
                CastDirection.z = 1;
                break;
            case Directions.f_right:
                CastDirection.x = 1;
                CastDirection.z = 1;
                break;
        }

        Debug.DrawRay(this.transform.position, transform.TransformDirection(CastDirection) * RaycastLength, Color.red);
        if(Physics.Raycast(this.transform.position, transform.TransformDirection(CastDirection), out hit, RaycastLength,  ~LayerMask.NameToLayer("Player")))
        {
            if(hit.collider != null)
            {
                Statistics s = hit.collider.GetComponent<Statistics>();
                if(s != null)
                {
                    if (RaycastedObject != hit.collider.gameObject)
                    {
                        s.DisplayStatistics();
                        RaycastedObject = hit.collider.gameObject;
                    }
                }
                else
                {
                    NoLongerRaycastingObject();
                }
            }
            else
            {
                NoLongerRaycastingObject();
            }
        }
        else
        {
            NoLongerRaycastingObject();
        }
    }

    void NoLongerRaycastingObject()
    {
        if (RaycastedObject != null)
        {
            RaycastedObject.GetComponent<Statistics>().HideStatistics();
        }
        RaycastedObject = null;
    }

    #endregion

}
