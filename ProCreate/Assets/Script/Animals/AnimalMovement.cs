using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    #region Movement variables

    [Header("Variables for Random Movement")]
    [Tooltip("If random value < ProbabilityToMove then animal will move")]
    [SerializeField] float ProbabilityToMove;
    [SerializeField] float DistanceToMove;
    [SerializeField] float MoveSpeed;
    [Tooltip("If Move < random value < Turn then animal will turn")]
    [SerializeField] float ProbabilityToTurn;
    [SerializeField] float DegreesToTurn;
    [SerializeField] float RotationSpeed;

    [SerializeField] float TimeBetweenMovements;

    float CurrentTimer;

    bool isPickedUp;

    #endregion

    [Header("Raycast Variables")]
    [SerializeField] float RaycastLength;


    private void Start()
    {
        CurrentTimer = TimeBetweenMovements;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isPickedUp)
        {
            DecrementTimer();
        }
    }

    void DecrementTimer()
    {
        if(CurrentTimer <= 0)
        {
            TryToMove();
            CurrentTimer = TimeBetweenMovements;
        }
        else
        {
            CurrentTimer -= Time.deltaTime;
        }
    }

    #region Movement Functions

    void TryToMove()
    {
        float val = Random.value;
        if(RaycastForObstacle())
        {
            if(val < ProbabilityToTurn)
            {
                StartCoroutine(TurnAnimal());
            }
            else
            {
                Debug.Log("Doing nothing");
            }
        }
        else
        {
            if(val < ProbabilityToMove)
            {
                StartCoroutine(MoveAnimal());
            }
            else if(val < ProbabilityToTurn)
            {
                StartCoroutine(TurnAnimal());
            }
            else
            {
                Debug.Log("Doing nothing");
            }
        }
    }

    IEnumerator MoveAnimal()
    {
        Vector3 TargetPos = this.transform.position + transform.forward * Random.Range(1, DistanceToMove);
        Debug.Log("Moving Animal");
        while(this.transform.position != TargetPos)
        {
            if(isPickedUp)
            {
                yield break;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, TargetPos, MoveSpeed * Time.deltaTime);
            yield return null;
            
        }
    }

    IEnumerator TurnAnimal()
    {
        float direction = Random.value < 0.5f ? -1 : 1;
        Debug.Log(transform.rotation.y * Mathf.Rad2Deg);
        Quaternion NewRotation = transform.rotation * Quaternion.AngleAxis(DegreesToTurn, Vector3.up);

        Debug.Log(NewRotation.y * Mathf.Rad2Deg);
        while (this.transform.rotation != NewRotation)
        {
            if (isPickedUp)
            {
                yield break;
            }
            this.transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, RotationSpeed * Time.deltaTime);
            yield return null;
        }

    }

    public void Pickedup()
    {
        isPickedUp = true;
    }

    public void SetDown()
    {
        isPickedUp = false;
    }

    #endregion

    #region Raycasting Functions
    bool RaycastForObstacle()
    {
        RaycastHit hit;

        Debug.DrawRay(this.transform.position + Vector3.up * 0.5f, transform.forward * RaycastLength, Color.red, 1f);
        if (Physics.Raycast(this.transform.position + Vector3.up, transform.forward, out hit, RaycastLength, ~(1 << LayerMask.NameToLayer("Animal"))))
        {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    #endregion


}
