using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalCanvas : GeneralCanvas
{
    [Header("UI Specific Objects")]
    [SerializeField] BarUI HungerBar;
    [SerializeField] BarUI ThirstBar;
    [SerializeField] BarUI WillBreedBar;
    [SerializeField] BarUI ResourceBar;
    [SerializeField] TextMeshProUGUI AnimalBreed;

    [Header("Camera Target")]
    [SerializeField] GameObject CanvasTarget;



    public delegate void CanUpdateUI();
    public CanUpdateUI CanUpdateCanvasUI;

    private void Awake()
    {
        this.transform.parent.GetComponent<AnimalMovement>().OpenCanvas += base.OpenCanvas;
        this.transform.parent.GetComponent<AnimalMovement>().CloseCanvas += base.CloseCanvas;
    }


    #region Changing UI Panel State

    protected override IEnumerator OpenCanvasRoutine()
    {
        SmoothCamera.instance.TargetCanvas(CanvasTarget.transform, true);
        yield return StartCoroutine(base.OpenCanvasRoutine());
        CanUpdateCanvasUI.Invoke();
    }

    protected override IEnumerator CloseCanvasRoutine()
    {
        yield return StartCoroutine(base.CloseCanvasRoutine());
        SmoothCamera.instance.TargetCanvas(CanvasTarget.transform, false);

    }

    #endregion

    #region Input Information into the UI Panel Components

    public void UpdateCanvas(float CurrentHungerPercent, float CurrentThirstPercent, float CurrentBreedPercent, float CurrentResourcePercent, string breed = "None")
    {
        HungerBar.UpdateProgressBar(CurrentHungerPercent);
        ThirstBar.UpdateProgressBar(CurrentThirstPercent);
        WillBreedBar.UpdateProgressBar(CurrentBreedPercent);
        ResourceBar.UpdateProgressBar(CurrentResourcePercent);
        AnimalBreed.text = breed;
    }

    #endregion
}
