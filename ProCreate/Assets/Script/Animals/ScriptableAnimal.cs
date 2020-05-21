using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableAnimal", menuName = "ScriptableAnimal")]
public class ScriptableAnimal : ScriptableObject
{
    [Header("Other Animal Info")]
    public string AnimalBreed;
    public int RewardMoney;

    [Header("Animal Stats: Breeding")]
    public float MaxWillingnessToBreed;
    public float RequiredWillingnessToBreedPercent;
    public float WillingnessChangeAmount;
    public float WillingnessChangeTimer;
    public float PostBreedPercent;

    [Header("Animal Stats: Health")]
    public float MaxFoodNeeded;
    public float MaxWaterNeeded;
    public float FoodDecrease;
    public float WaterDecrease;
    public float FoodDecreaseTimer;
    public float WaterDecreaseTimer;

    [Header("Animal Resource Variables")]
    public float ResourceIncrease;
    public float ResourceTimer;
    public float ResourceValue;
}
