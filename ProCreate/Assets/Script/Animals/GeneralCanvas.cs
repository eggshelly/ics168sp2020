using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCanvas : MonoBehaviour
{
    [Header("UI General Objects")]
    [SerializeField] RectTransform CanvasRect;
    [SerializeField] GameObject UIPanel;

    [Header("UI General Variables")]
    [SerializeField] protected float CanvasHeight = 100;
    [SerializeField] protected float CanvasMinY = 2;
    [SerializeField] protected float CanvasMaxY = 6;
    [SerializeField] protected float CanvasMovementMultiplier = 5;

    #region Open/Close Variables

    bool CanvasInTransition = false;
    bool StopCurrentRoutine = false;
    bool CanvasOpened = false;

    Vector3 OriginalPos;

    #endregion

    #region Built In Functions


    private void Start()
    {
        CanvasRect.gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, 0);
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMinY, CanvasRect.position.z);
        OriginalPos = CanvasRect.position;
        UIPanel.SetActive(false);
        CanvasRect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (CanvasOpened)
        {
            LookAtCamera();
        }
    }

    #endregion

    #region Changing UI Panel State

    protected void ToggleCanvas()
    {
        if (CanvasOpened)
        {
            CloseCanvas();
        }
        else
        {
            OpenCanvas();
        }
    }


    void OpenCanvas()
    {
        StartCoroutine(OpenCanvasRoutine());
    }

    void CloseCanvas()
    {
        StartCoroutine(CloseCanvasRoutine());
    }

    protected virtual IEnumerator OpenCanvasRoutine()
    {
        if (CanvasInTransition)
        {
            StopCurrentRoutine = true;
            while (StopCurrentRoutine)
                yield return null;
        }


        LookAtCamera();
        CanvasOpened = true;
        CanvasRect.gameObject.SetActive(true);
        CanvasInTransition = true;
        while (CanvasRect.sizeDelta.y < CanvasHeight)
        {
            CanvasRect.sizeDelta += Vector2.up * CanvasHeight * Time.deltaTime * CanvasMovementMultiplier;
            CanvasRect.position += Vector3.up * (CanvasMaxY - CanvasMinY) * Time.deltaTime * CanvasMovementMultiplier;
            yield return null;
            if (StopCurrentRoutine)
            {
                break;
            }
        }
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, CanvasHeight);
        CanvasRect.localPosition = Vector3.zero;
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMaxY, CanvasRect.position.z);
        CanvasInTransition = false;
        UIPanel.SetActive(true);
        StopCurrentRoutine = false;
    }

    protected virtual IEnumerator CloseCanvasRoutine()
    {
        if (CanvasInTransition)
        {
            StopCurrentRoutine = true;
            while (StopCurrentRoutine)
                yield return null;
        }


        CanvasOpened = false;
        CanvasInTransition = true;
        UIPanel.SetActive(false);
        while (CanvasRect.sizeDelta.y > 0)
        {
            CanvasRect.sizeDelta -= Vector2.up * CanvasHeight * Time.deltaTime * CanvasMovementMultiplier;
            CanvasRect.position -= Vector3.up * (CanvasMaxY - CanvasMinY) * Time.deltaTime * CanvasMovementMultiplier;
            yield return null;
            if (StopCurrentRoutine)
            {
                break;
            }
        }
        CanvasRect.sizeDelta = new Vector2(CanvasRect.sizeDelta.x, 0);
        CanvasRect.localPosition = Vector3.zero;
        CanvasRect.position = new Vector3(CanvasRect.position.x, CanvasMinY, CanvasRect.position.z);
        CanvasInTransition = false;
        CanvasRect.gameObject.SetActive(false);
        StopCurrentRoutine = false;
    }

    void LookAtCamera()
    {
        Vector3 pos = this.transform.position - Camera.main.transform.position;
        this.transform.rotation = Quaternion.LookRotation(pos, Vector3.up); /* + this.transform.position.y) *(3/4f), Camera.main.transform.position.z)));*/
    }

    #endregion
}
