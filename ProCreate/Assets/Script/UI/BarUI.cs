using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
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

    public void UpdateProgressBar(float percent)
    {
        float posDiff = width - (width * percent);
        float widthDiff = (width * 2) - (width * 2 * percent);

        ProgressBar.localPosition += Vector3.left * posDiff;
        ProgressBar.sizeDelta += Vector2.left * widthDiff;
    }

    private void OnDisable()
    {
        ProgressBar.localPosition = OriginalPosition;
        ProgressBar.sizeDelta = OriginalSizeDelta;
    }
}
