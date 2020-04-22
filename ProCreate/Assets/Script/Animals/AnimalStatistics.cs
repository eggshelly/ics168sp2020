using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatistics : MonoBehaviour, Statistics, Animal
{
    [Header("Animal UI")]
    [SerializeField] AnimalCanvas AnimalCanvas;

    [Header("Animal Health")]
    [SerializeField] ScriptableAnimal Animal;

    #region Animal Stats

    [SerializeField] float StartingHungerPercent;
    [SerializeField] float StartingThirstPercent;
    [SerializeField] float CurrentHunger;
    [SerializeField] float CurrentThirst;
    [SerializeField] string Nickname;
    #endregion

    #region Animal Breeding Variables
    [Header("Breeding Variables")]
    [SerializeField] float StartingWTBPercent;
    [SerializeField] float CurrentWillingnessToBreed;


    #endregion

    #region Other Variables Needed to Update Stats

    float ThirstTimer;
    float HungerTimer;
    float WillingnessTimer;

    #endregion




    // Start is called before the first frame update
    void Start()
    {
        CurrentHunger = StartingHungerPercent * Animal.MaxFoodNeeded;
        CurrentThirst = StartingThirstPercent * Animal.MaxWaterNeeded;
        CurrentWillingnessToBreed = StartingWTBPercent * Animal.MaxWillingnessToBreed;
        ThirstTimer = Animal.WaterDecreaseTimer;
        HungerTimer = Animal.FoodDecreaseTimer;
        WillingnessTimer = Animal.WillingnessChangeTimer;
        AnimalCanvas.CanUpdateCanvasUI += UpdateCanvasUI;
    }

    private void Update()
    {
        DecrementHungerAndThirst();
        ChangeWillingnessToBreed();
    }

    #region Changing Current Hunger and Thirst

    void DecrementHungerAndThirst()
    {
        if(ThirstTimer <= 0)
        {
            CurrentThirst -= Animal.WaterDecrease;
            CurrentThirst = (CurrentThirst >= 0 ? CurrentThirst : 0);
            ThirstTimer = Animal.WaterDecreaseTimer;
        }
        else
        {
            ThirstTimer -= Time.deltaTime;
        }
        if(HungerTimer <= 0)
        {
            CurrentHunger -= Animal.FoodDecrease;
            CurrentHunger = (CurrentHunger >= 0 ? CurrentHunger : 0);
            HungerTimer = Animal.FoodDecreaseTimer;
        }
        else
        {
            HungerTimer -= Time.deltaTime;
        }

    }

    public void GiveFoodAndWater(float food, float water)
    {
        Debug.Log("Original Hunger: " + CurrentHunger.ToString() + " Original Thirst: " + CurrentThirst.ToString());
        CurrentHunger += food;
        CurrentThirst += water;

        Debug.Log("New Hunger: " + CurrentHunger.ToString() + " New Thirst: " + CurrentThirst.ToString());
        CurrentHunger = (CurrentHunger > Animal.MaxFoodNeeded ? Animal.MaxFoodNeeded : CurrentHunger);
        CurrentThirst = (CurrentThirst > Animal.MaxWaterNeeded ? Animal.MaxWaterNeeded : CurrentThirst);

        UpdateCanvasUI();
    }

    #endregion

    #region Changing Breeding Stats
    void ChangeWillingnessToBreed()
    {
        if (WillingnessTimer <= 0)
        {
            float HungerPercent = (CurrentHunger / Animal.MaxFoodNeeded);
            float ThirstPercent = (CurrentThirst / Animal.MaxWaterNeeded);

            if (HungerPercent < 0.5f || ThirstPercent < 0.3f)
            {
                CurrentWillingnessToBreed -= Animal.WillingnessChangeAmount;
            }
            else
            {
                CurrentWillingnessToBreed += Animal.WillingnessChangeAmount;
            }
            CurrentWillingnessToBreed = (CurrentWillingnessToBreed > Animal.MaxWillingnessToBreed ? Animal.MaxWillingnessToBreed : (CurrentWillingnessToBreed < 0 ? 0 : CurrentWillingnessToBreed));
            WillingnessTimer = Animal.WillingnessChangeTimer;
        }
        else
        {
            WillingnessTimer -= Time.deltaTime;
        }

    }

    public bool isWillingToBreed()
    {
        return (CurrentWillingnessToBreed / Animal.MaxWillingnessToBreed) > Animal.RequiredWillingnessToBreedPercent;
    }

    #endregion


    #region Canvas Related Functions

    #region Updating the Canvas objects

    public void UpdateCanvasUI()
    {
        AnimalCanvas.UpdateCanvas((CurrentHunger / Animal.MaxFoodNeeded), (CurrentThirst / Animal.MaxWaterNeeded), (CurrentWillingnessToBreed / Animal.MaxWillingnessToBreed), Nickname, Animal.AnimalBreed);
    }
    #endregion

    #region Open/Close Canvas

    public void DisplayStatistics()
    {
        AnimalCanvas.OpenStatisticsUI();
    }

    public void HideStatistics()
    {
        AnimalCanvas.CloseStatisticsUI();
    }
    #endregion

    #endregion
}
