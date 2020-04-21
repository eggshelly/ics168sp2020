using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableAnimal", menuName = "ScriptableAnimal")]
public class ScriptableAnimal : ScriptableObject
{
    [Header("Animal Stats: Type")]
    public string AnimalBreed;
    public string AnimalFamily;

    [Header("Animal Stats: Breeding")]
    public float RequiredWillingnessToBreed;
    public float WillingnessIncreaseAmount;

    [Header("Animal Stats: Health")]
    public float MaxFoodNeeded;
    public float MaxWaterNeeded;
    public float FoodDecrease;
    public float WaterDecrease;
    public float FoodDecreaseTimer;
    public float WaterDecreaseTimer;
}
