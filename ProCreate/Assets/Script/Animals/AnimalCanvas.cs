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

    [Header("Camera Target")]
    [SerializeField] GameObject CanvasTarget;



    public delegate void CanUpdateUI();
    public CanUpdateUI CanUpdateCanvasUI;

    private void Awake()
    {
        Debug.Log(this.transform.parent == null ? "Null" : this.transform.parent.name);
        this.transform.parent.GetComponent<AnimalMovement>().ToggleCanvas += base.ToggleCanvas;
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
        yield return base.CloseCanvasRoutine();
        SmoothCamera.instance.TargetCanvas(CanvasTarget.transform, false);

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
