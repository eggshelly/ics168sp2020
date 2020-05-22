﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatistics : MonoBehaviour, Animal
{
    [Header("Animal UI")]
    [SerializeField] AnimalCanvas AnimalCanvas;

    [Header("Animal Stats")]
    [SerializeField] ScriptableAnimal Animal;

    #region AnimalInfo

    int NewBreedReward;
    string Breed;

    #endregion

    #region Animal Stats
    [SerializeField]  float StartingHungerPercent;
    [SerializeField]  float StartingThirstPercent;
    [SerializeField]  float CurrentHunger = 0f;
    [SerializeField]  float CurrentThirst = 0f;


    float MaxFoodNeeded;
    float MaxWaterNeeded;
    float FoodDecrease;
    float WaterDecrease;
    float FoodDecreaseTimer;
    float WaterDecreaseTimer;
    #endregion

    #region Animal Resource Variables
    [Header("Animal Resource Variables")]
    [SerializeField] float StartingResourcePercent = 0.3f;
    [SerializeField] float CurrentResourceReadiness;

    float ResourceDecreaseTimer;
    float ResourceIncrease;
    float MaxResource = 100f;

    float ResourceValue;
    #endregion

    #region Animal Breeding Variables
    [Header("Breeding Variables")]
    [SerializeField]  float StartingWTBPercent = 0.8f;
    [SerializeField]  float CurrentWillingnessToBreed = 0;


    float MaxWillingnessToBreed;
    float RequiredWillingsnessToBreedPercent;
    float WillingnessChangeAmount;
    float WillingnessChangeTimer;
    float PostBreedPercent;
    #endregion

    #region Other Variables Needed to Update Stats

    float ThirstTimer;
    float HungerTimer;
    float WillingnessTimer;
    float ResourceTimer;

    #endregion


    #region Built In / Setup Functions

    // Start is called before the first frame update
    void Start()
    {
        SetupVariables();
        CurrentResourceReadiness = (CurrentResourceReadiness == 0 ? StartingResourcePercent * MaxResource : CurrentResourceReadiness);
        CurrentHunger = (CurrentHunger == 0 ? StartingHungerPercent * MaxFoodNeeded : CurrentHunger);
        CurrentThirst = (CurrentThirst == 0 ? StartingThirstPercent * MaxWaterNeeded : CurrentThirst);
        CurrentWillingnessToBreed = (CurrentWillingnessToBreed  == 0 ? StartingWTBPercent * MaxWillingnessToBreed : CurrentWillingnessToBreed);
        ThirstTimer = WaterDecreaseTimer;
        HungerTimer = FoodDecreaseTimer;
        ResourceTimer = ResourceDecreaseTimer;
        WillingnessTimer = WillingnessChangeTimer;
        AnimalCanvas.CanUpdateCanvasUI += UpdateCanvasUI;
    }



    private void Update()
    {
        DecrementHungerAndThirst();
        DecrementResourceTimer();
        ChangeWillingnessToBreed();
    }

    void SetupVariables()
    {
        MaxFoodNeeded = (Animal != null ? Animal.MaxFoodNeeded : MaxFoodNeeded);
        MaxWaterNeeded = (Animal != null ? Animal.MaxWaterNeeded : MaxWaterNeeded);
        FoodDecrease = (Animal != null ? Animal.FoodDecrease : FoodDecrease);
        WaterDecrease = (Animal != null ? Animal.WaterDecrease : WaterDecrease);
        FoodDecreaseTimer = (Animal != null ? Animal.FoodDecreaseTimer : FoodDecreaseTimer);
        WaterDecreaseTimer = (Animal != null ? Animal.WaterDecreaseTimer : WaterDecreaseTimer);
        NewBreedReward = (Animal != null ? Animal.RewardMoney : NewBreedReward);
        Breed = (Animal != null ? Animal.AnimalBreed : Breed);

        MaxWillingnessToBreed = (Animal != null ? Animal.MaxWillingnessToBreed : MaxWillingnessToBreed);
        RequiredWillingsnessToBreedPercent = (Animal != null ? Animal.RequiredWillingnessToBreedPercent : RequiredWillingsnessToBreedPercent);
        WillingnessChangeAmount = (Animal != null ? Animal.WillingnessChangeAmount : WillingnessChangeAmount);
        WillingnessChangeTimer = (Animal != null ? Animal.WillingnessChangeTimer : WillingnessChangeTimer);
        PostBreedPercent = (Animal != null ? Animal.PostBreedPercent : PostBreedPercent);

        ResourceIncrease = (Animal != null ? Animal.ResourceIncrease : ResourceIncrease);
        ResourceDecreaseTimer = (Animal != null ? Animal.ResourceTimer : ResourceDecreaseTimer);
        ResourceValue = (Animal != null ? Animal.ResourceValue : ResourceValue);
    }

    #endregion

    #region Changing Resource Variables

    void DecrementResourceTimer()
    {
        if(ResourceTimer <= 0)
        {
            CurrentResourceReadiness += ResourceIncrease;
            CurrentResourceReadiness = (CurrentResourceReadiness >= 100 ? 100 : CurrentResourceReadiness);
            ResourceTimer = ResourceDecreaseTimer;
        }
        else
        {
            ResourceTimer -= Time.deltaTime;
        }
    }

    public void RetrieveResourceFromAnimal()
    {
        PlayerManager.instance.ChangeMoneyAmount((int)ResourceValue);
        CurrentResourceReadiness = 0f;
        UpdateCanvasUI();
    }

    #endregion


    #region Changing Current Hunger and Thirst

    void DecrementHungerAndThirst()
    {
        if(ThirstTimer <= 0)
        {
            CurrentThirst -= WaterDecrease;
            CurrentThirst = (CurrentThirst >= 0 ? CurrentThirst : 0);
            ThirstTimer = WaterDecreaseTimer;
        }
        else
        {
            ThirstTimer -= Time.deltaTime;
        }
        if(HungerTimer <= 0)
        {
            CurrentHunger -= FoodDecrease;
            CurrentHunger = (CurrentHunger >= 0 ? CurrentHunger : 0);
            HungerTimer = FoodDecreaseTimer;
        }
        else
        {
            HungerTimer -= Time.deltaTime;
        }

    }

    public void GiveFoodAndWater(float food, float water)
    {
        CurrentHunger += food;
        CurrentThirst += water;

        CurrentHunger = (CurrentHunger > MaxFoodNeeded ? MaxFoodNeeded : CurrentHunger);
        CurrentThirst = (CurrentThirst > MaxWaterNeeded ? MaxWaterNeeded : CurrentThirst);

        UpdateCanvasUI();
    }

    #endregion

    #region Changing Breeding Stats

    void ChangeWillingnessToBreed()
    {
        if (WillingnessTimer <= 0)
        {
            float HungerPercent = (CurrentHunger / MaxFoodNeeded);
            float ThirstPercent = (CurrentThirst / MaxWaterNeeded);

            if (HungerPercent < 0.5f || ThirstPercent < 0.3f)
            {
                CurrentWillingnessToBreed -= WillingnessChangeAmount;
            }
            else
            {
                CurrentWillingnessToBreed += WillingnessChangeAmount;
            }
            CurrentWillingnessToBreed = (CurrentWillingnessToBreed > MaxWillingnessToBreed ? MaxWillingnessToBreed : (CurrentWillingnessToBreed < 0 ? 0 : CurrentWillingnessToBreed));
            WillingnessTimer = WillingnessChangeTimer;
        }
        else
        {
            WillingnessTimer -= Time.deltaTime;
        }

    }

    public bool IsWillingToBreed()
    {
        return (CurrentWillingnessToBreed / MaxWillingnessToBreed) > RequiredWillingsnessToBreedPercent;
    }

    public void FinishedBreeding()
    {
        CurrentWillingnessToBreed = 0f;//MaxWillingnessToBreed * StartingWTBPercent;
    }

    public void IsNewChild( float StartWillPer, float MaxWillBreed, float ReqWillBreed, float WillChangeAm, float WillChangeTimer, float PostBreedPer, float StartHungPer, float StartThirstPer, 
        float MaxFood, float MaxWater, float FoodDec, float WaterDec, float FoodDecTimer, float WaterDecTimer, float ResDecTimer, float RecInc, float ResVal, int reward, string breed)
    {
        StartingWTBPercent = StartWillPer;
        MaxWillingnessToBreed = MaxWillBreed;
        RequiredWillingsnessToBreedPercent = ReqWillBreed;
        WillingnessChangeAmount = WillChangeAm;
        WillingnessChangeTimer = WillChangeTimer;
        PostBreedPercent = PostBreedPer;
        StartingHungerPercent = StartHungPer;
        StartingThirstPercent = StartThirstPer;


        MaxFoodNeeded = MaxFood;
        MaxWaterNeeded = MaxWater;
        FoodDecrease = FoodDec;
        WaterDecrease = WaterDec;
        FoodDecreaseTimer = FoodDecTimer;
        WaterDecreaseTimer = WaterDecTimer;

        ResourceDecreaseTimer = ResDecTimer;
        ResourceIncrease = RecInc;
        ResourceValue = ResVal;

        NewBreedReward = reward;
        this.Breed = breed;

        Utilities.RecursivelySetLayer(this.gameObject.transform, LayerMask.NameToLayer("Animal"));

        PlayerManager.instance.ChangeMoneyAmount(NewBreedReward);
    }

    #endregion

    #region Updating the Canvas objects

    public void UpdateCanvasUI()
    {
        AnimalCanvas.UpdateCanvas((CurrentHunger / MaxFoodNeeded), (CurrentThirst / MaxWaterNeeded), (CurrentWillingnessToBreed / MaxWillingnessToBreed), (CurrentResourceReadiness / MaxResource), this.Breed);
    }

    public void SetAnimalCanvas(GameObject CanvasPrefab)
    {
        GameObject NewCanvas = Instantiate(CanvasPrefab, this.transform);
        this.AnimalCanvas = NewCanvas.GetComponent<AnimalCanvas>();
    }
    #endregion

    #region Get Attributes
    public void setCanvas(AnimalCanvas other)
    {
        AnimalCanvas = other;
    }
    public void setStartingWTB(float other)
    {
        StartingWTBPercent = other;
    }

    public float GetStartingWTB()
    {
        return StartingWTBPercent;
    }

    public float GetMaxWTB()
    {
        return MaxWillingnessToBreed;
    }

    public float GetReqWTBPercent()
    {
        return RequiredWillingsnessToBreedPercent;
    }

    public float GetWillChangeAm()
    {
        return WillingnessChangeAmount;
    }

    public float GetWillChangeTimer()
    {
        return WillingnessChangeTimer;
    }

    public float GetPostBreedPercent()
    {
        return PostBreedPercent;
    }

    public float GetStartHungPer()
    {
        return StartingHungerPercent;
    }

    public float GetStartThirstPer()
    {
        return StartingThirstPercent;
    }

    public float GetMaxFood()
    {
        return MaxFoodNeeded;
    }

    public float GetMaxWater()
    {
        return MaxWaterNeeded;
    }

    public float GetFoodDec()
    {
        return FoodDecrease;
    }

    public float GetWaterDec()
    {
        return WaterDecrease;
    }

    public float GetFoodDecTimer()
    {
        return FoodDecreaseTimer;
    }
    
    public float GetWaterDecTimer()
    {
        return WaterDecreaseTimer;
    }

    public float GetRewardValue()
    {
        return NewBreedReward;
    }

    public string GetBreed()
    {
        return Breed;
    }

    public float GetResDecTimer()
    {
        return ResourceDecreaseTimer;
    }

    public float GetResourceIncrease()
    {
        return ResourceIncrease;
    }

    public float GetResourceValue()
    {
        return ResourceValue;
    }

    #endregion


}
