using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatistics : MonoBehaviour, Statistics
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




    // Start is called before the first frame update
    void Start()
    {
        CurrentHunger = StartingHungerPercent * Animal.MaxFoodNeeded;
        CurrentThirst = StartingThirstPercent * Animal.MaxWaterNeeded;
        AnimalCanvas.CanUpdateCanvasUI += UpdateCanvasUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCanvasUI()
    {
        AnimalCanvas.UpdateCanvas((CurrentHunger / Animal.MaxFoodNeeded), (CurrentThirst / Animal.MaxWaterNeeded), Nickname, Animal.AnimalBreed);
    }

    public void DisplayStatistics()
    {
        AnimalCanvas.OpenStatisticsUI();
    }

    public void HideStatistics()
    {
        AnimalCanvas.CloseStatisticsUI();
    }
}
