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
    [SerializeField] float SpeedMultiplier = 0.25f;
    [SerializeField] Directions FacingDirection = Directions.neutral;

    #endregion

    #region Raycast Variables
    [SerializeField] float RaycastLength = 2f;

    GameObject RaycastedObject;

    #endregion

    private void FixedUpdate()
    {
        MovePlayer();
    }

    #region Movement


    //Moves the transform of the player 
    void MovePlayer()
    {
        Vector2 directions = ChangePlayerDirection();

        this.transform.position += (Vector3.right * directions.x + Vector3.forward * directions.y) * Time.deltaTime * SpeedMultiplier;
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

    public Directions GetCurrentDirection()
    {
        return FacingDirection;
    }
    #endregion


}
