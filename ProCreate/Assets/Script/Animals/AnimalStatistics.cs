using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatistics : MonoBehaviour, Statistics
{
    [Header("Animal UI")]
    [SerializeField] AnimalCanvas AnimalCanvas;

    [Header("Animal Stats: Breeding")]
    [SerializeField] float RequiredWillingnessToBreed;
    [SerializeField] float WillingnessIncreaseAmount;

    [Header("Animal Stats: Health")]
    [SerializeField] float MaxFoodNeeded;
    [SerializeField] float MaxWaterNeeded;
    [SerializeField] float MaxHealth;
    [SerializeField] float FoodDecrease;
    [SerializeField] float WaterDecrease;
    [SerializeField] float HealthDecrease;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayStatistics()
    {
        AnimalCanvas.gameObject.SetActive(true);
    }

    public void HideStatistics()
    {
        AnimalCanvas.gameObject.SetActive(false);
    }
}
