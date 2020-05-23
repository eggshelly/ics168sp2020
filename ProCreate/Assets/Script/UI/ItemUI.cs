using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemCost;
    [SerializeField] Image ItemSprite;

    [Header("Background")]
    [SerializeField] GameObject Selected;
    [SerializeField] GameObject NotSelected;

    RectTransform rect;

    private void Awake()
    {
        rect = this.GetComponent<RectTransform>();
    }


    public void SetupUI(string name = "", int cost = 0, Sprite sprite = null)
    {
        ItemName.text = name;
        ItemCost.text = cost.ToString();
        ItemSprite.color = Random.ColorHSV();
    }


    public void SelectItem()
    {
        Selected.transform.SetAsLastSibling();
    }

    public void DeselectItem()
    {
        NotSelected.transform.SetAsLastSibling();
    }

    public Vector3 GetRectPosition()
    {
        return rect.position;
    }

    public Vector3 GetAnchoredPosition()
    {
        return rect.anchoredPosition;
    }
}
