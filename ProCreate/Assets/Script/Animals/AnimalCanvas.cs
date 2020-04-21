using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalCanvas : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] RectTransform CanvasRect;
    [SerializeField] GameObject UIPanel;
    [SerializeField] BarUI HungerBar;
    [SerializeField] BarUI ThirstBar;
    [SerializeField] TextMeshProUGUI AnimalNickname;
    [SerializeField] TextMeshProUGUI AnimalBreed;

    [Header("UI Variables")]
    [SerializeField] float CanvasHeight;
    [SerializeField] float CanvasMinY;
    [SerializeField] float CanvasMaxY;
    [SerializeField] float CanvasMovementMultiplier;

    public delegate void CanUpdateUI();
    public CanUpdateUI CanUpdateCanvasUI;


    #region Open/Close Variables

    bool CanvasInTransition = false;
    bool StopCurrentRoutine = false;
    bool CanvasOpened = false;

    #endregion

    private void Awake()
    {
        LookAtCamera();
        CanvasRect.gameObject.SetActive(false);
        UIPanel.SetActive(false);
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, 0);
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMinY, CanvasRect.position.z);
    }

    void LookAtCamera()
    {
        Vector3 pos = this.transform.position - Camera.main.transform.position;
        this.transform.rotation = Quaternion.LookRotation(pos); /* + this.transform.position.y) *(3/4f), Camera.main.transform.position.z)));*/
    }

    #region Open/Close the Animal UI Panel

    public void OpenStatisticsUI()
    {
        StartCoroutine(OpenStatisticsUIRoutine());
    }
    
    public void CloseStatisticsUI()
    {
        StartCoroutine(CloseStatisticsUIRoutine());
    }

    IEnumerator OpenStatisticsUIRoutine()
    {
        LookAtCamera();
        CanvasRect.gameObject.SetActive(true);
        if (CanvasInTransition)
        {
            StopCurrentRoutine = true;
            while (StopCurrentRoutine)
                yield return null;
        }
        CanvasInTransition = true;
        while(CanvasRect.sizeDelta.y < CanvasHeight)
        {
            CanvasRect.sizeDelta += Vector2.up * CanvasHeight * Time.deltaTime * CanvasMovementMultiplier;
            CanvasRect.position += Vector3.up * (CanvasMaxY - CanvasMinY) * Time.deltaTime * CanvasMovementMultiplier;
            yield return null;
            if(StopCurrentRoutine)
            {
                StopCurrentRoutine = false;
                yield break;
            }
        }
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, CanvasHeight);
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMaxY, CanvasRect.position.z);
        CanvasOpened = true;
        CanvasInTransition = false;
        UIPanel.SetActive(true);
        CanUpdateCanvasUI.Invoke();
    }

    IEnumerator CloseStatisticsUIRoutine()
    {
        if (CanvasInTransition)
        {
            StopCurrentRoutine = true;
            while (StopCurrentRoutine)
                yield return null;
        }
        CanvasInTransition = true;
        UIPanel.SetActive(false);
        while (CanvasRect.sizeDelta.y > 0)
        {
            CanvasRect.sizeDelta -= Vector2.up * CanvasHeight * Time.deltaTime * CanvasMovementMultiplier;
            CanvasRect.position -= Vector3.up * (CanvasMaxY - CanvasMinY) * Time.deltaTime * CanvasMovementMultiplier;
            yield return null;
            if (StopCurrentRoutine)
            {
                StopCurrentRoutine = false;
                yield break;
            }
        }
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, 0);
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMinY, CanvasRect.position.z);
        CanvasOpened = true;
        CanvasInTransition = false;
        CanvasRect.gameObject.SetActive(false);
    }
    #endregion

    #region Input Information into the UI Panel Components

    public void UpdateCanvas(float CurrentHungerPercent, float CurrentThirstPercent, string nickname, string breed)
    {
        HungerBar.UpdateProgressBar(CurrentHungerPercent);
        ThirstBar.UpdateProgressBar(CurrentThirstPercent);
        AnimalNickname.text = nickname;
        AnimalBreed.text = breed;
    }

    #endregion
}
