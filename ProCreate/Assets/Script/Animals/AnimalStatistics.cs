using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatistics : MonoBehaviour, Statistics, Animal
{
    [Header("Animal UI")]
    [SerializeField] AnimalCanvas AnimalCanvas;

    [Header("Animal")]
    [SerializeField] ScriptableAnimal Animal;

    #region Animal Stats

    [SerializeField] float StartingHungerPercent;
    [SerializeField] float StartingThirstPercent;
    [SerializeField] float CurrentHunger;
    [SerializeField] float CurrentThirst;
    [SerializeField] string Nickname;
    #endregion

    #region Other Variables Needed to Update Stats

    float ThirstTimer;
    float HungerTimer;

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        CurrentHunger = StartingHungerPercent * Animal.MaxFoodNeeded;
        CurrentThirst = StartingThirstPercent * Animal.MaxWaterNeeded;
        ThirstTimer = Animal.WaterDecreaseTimer;
        HungerTimer = Animal.FoodDecreaseTimer;
        AnimalCanvas.CanUpdateCanvasUI += UpdateCanvasUI;
    }

    private void Update()
    {
        DecrementHungerAndThirst();
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
    //In progress

    #endregion


    #region Canvas Related Functions

    #region Updating the Canvas objects

    public void UpdateCanvasUI()
    {
        AnimalCanvas.UpdateCanvas((CurrentHunger / Animal.MaxFoodNeeded), (CurrentThirst / Animal.MaxWaterNeeded), Nickname, Animal.AnimalBreed);
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
