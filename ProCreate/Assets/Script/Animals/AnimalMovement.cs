using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    #region Movement variables

    [Header("Variables for Random Movement")]
    [Tooltip("If random value < ProbabilityToMove then animal will move")]
    [SerializeField] public float ProbabilityToMove = 0.5f;
    [SerializeField] public float DistanceToMove;
    [SerializeField] public float MoveSpeed;
    [Tooltip("If Move < random value < Turn then animal will turn")]
    [SerializeField] public float ProbabilityToTurn = 0.8f;
    [SerializeField] public float DegreesToTurn;
    [SerializeField] public float RotationSpeed;

    [SerializeField] float TimeBetweenMovements;

    float CurrentTimer;

    bool isPickedUp;

    bool InTransition = false;

    #endregion

    #region Raycast Variables

    [Header("Raycast Variables")]
    [SerializeField] float RaycastLength;

    bool PlayerIsNear = false;

    #endregion

    #region Components

    AnimalStatistics AnimalStats;

    BoxCollider coll;

    #endregion

    #region Built In / Setup Functions


    private void Start()
    {
        coll = this.GetComponent<BoxCollider>();
        AnimalStats = this.GetComponent<AnimalStatistics>();
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

    private void FixedUpdate()
    {
        CheckIfPlayerIsNear();
    }

    void DecrementTimer()
    {
        if (CurrentTimer <= 0)
        {
            TryToMove();
            CurrentTimer = TimeBetweenMovements;
        }
        else
        {
            CurrentTimer -= Time.deltaTime;
        }
    }

    public void SetVariables(float MoveDist, float MoveSpeed, float TurnDeg, float TurnSpeed, float BetweenTime)
    {
        this.DistanceToMove = MoveDist;
        this.MoveSpeed = MoveSpeed;
        this.DegreesToTurn = TurnDeg;
        this.RotationSpeed = TurnSpeed;
        this.TimeBetweenMovements = BetweenTime;
    }

    #endregion


    #region Movement Functions

    void TryToMove()
    {
        if(InTransition)
        {
            return;
        }

        float val = Random.value;
        if(RaycastForObstacle())
        {
            if(val < ProbabilityToTurn)
            {
                StartCoroutine(TurnAnimal());
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
        }
    }

    IEnumerator MoveAnimal()
    {
        InTransition = true;
        Vector3 TargetPos = this.transform.position + transform.forward * Random.Range(1, DistanceToMove);
        while(Mathf.Abs(this.transform.position.sqrMagnitude - TargetPos.sqrMagnitude) > 0.2f)
        {
            if (isPickedUp)
            {
                InTransition = false;
                yield break;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, TargetPos, MoveSpeed * Time.deltaTime);
            yield return null;
            
        }
        this.transform.position = TargetPos;
        InTransition = false;
        Debug.Log("Finished with moving");
    }

    IEnumerator TurnAnimal()
    {
        InTransition = true;
        float direction = Random.value < 0.5f ? -1 : 1;
        Quaternion NewRotation = transform.rotation * Quaternion.AngleAxis(DegreesToTurn, Vector3.up);
        while (Mathf.Abs(this.transform.rotation.eulerAngles.sqrMagnitude - NewRotation.eulerAngles.sqrMagnitude) > 0.5f)
        {
            if (isPickedUp)
            {
                yield break;
            }
            this.transform.rotation = Quaternion.Lerp(transform.rotation, NewRotation, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        this.transform.rotation = NewRotation;
        InTransition = false;
        Debug.Log("Finished with turning");

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

    void CheckIfPlayerIsNear()
    {
        if(!isPickedUp)
        {
            Collider[] colls = Physics.OverlapSphere(this.transform.position + coll.center, 2f, 1 << LayerMask.NameToLayer("Player"));
            if(colls.Length > 0)
            {
                if (!PlayerIsNear)
                {
                    AnimalStats.DisplayStatistics();
                    PlayerIsNear = true;
                }
            }
            else
            {
                if(PlayerIsNear)
                {
                    AnimalStats.HideStatistics();
                    PlayerIsNear = false;
                }
            }
        }
        else
        {
            if (PlayerIsNear)
            {
                AnimalStats.HideStatistics();
                PlayerIsNear = false;
            }

        }
    }

    #endregion

    #region Get Attributes

    public float GetMoveDist()
    {
        return DistanceToMove;
    }

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }

    public float GetDegTurn()
    {
        return DegreesToTurn;
    }

    public float GetRotSpeed()
    {
        return RotationSpeed;
    }

    public float GetMoveTimer()
    {
        return TimeBetweenMovements;
    }

    #endregion


}
