using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    [Header("Objects to Adjust")]
    [SerializeField] RectTransform ProgressBar;

    float width;
    Vector2 OriginalSizeDelta;
    Vector3 OriginalPosition;

    private void Awake()
    {
        width = ProgressBar.rect.width / 2;
        OriginalSizeDelta = ProgressBar.sizeDelta;
        OriginalPosition = ProgressBar.localPosition;
    }

    //Updates the pos/sizeDelta of the ProgressBar to simulate decreasing the gauge
    public void UpdateProgressBar(float percent)
    {
        float posDiff = width - (width * percent);
        float widthDiff = (width * 2) - (width * 2 * percent);

        ProgressBar.localPosition = OriginalPosition + Vector3.left * posDiff;
        ProgressBar.sizeDelta = OriginalSizeDelta + Vector2.left * widthDiff;
    }

    private void OnDisable()
    {
        ProgressBar.localPosition = OriginalPosition;
        ProgressBar.sizeDelta = OriginalSizeDelta;
    }
}
