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
    [SerializeField] TextMeshProUGUI AnimalNickname;
    [SerializeField] TextMeshProUGUI AnimalBreed;



    public delegate void CanUpdateUI();
    public CanUpdateUI CanUpdateCanvasUI;

    private void Awake()
    {
        this.transform.parent.GetComponent<AnimalMovement>().ToggleCanvas += base.ToggleCanvas;
    }


    #region Changing UI Panel State

    protected override IEnumerator OpenCanvasRoutine()
    {
        yield return StartCoroutine(base.OpenCanvasRoutine());
        CanUpdateCanvasUI.Invoke();
    }

    #endregion

    #region Input Information into the UI Panel Components

    public void UpdateCanvas(float CurrentHungerPercent, float CurrentThirstPercent, float CurrentBreedPercent, string nickname = "None", string breed = "None")
    {
        HungerBar.UpdateProgressBar(CurrentHungerPercent);
        ThirstBar.UpdateProgressBar(CurrentThirstPercent);
        WillBreedBar.UpdateProgressBar(CurrentBreedPercent);
        AnimalNickname.text = nickname;
        AnimalBreed.text = breed;
    }

    #endregion
}
