using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGrid : MonoBehaviour
{
    [Header("Items In Shop")]
    [SerializeField] List<ScriptableItem> Items;

    [Header("Objects for Shop Setup")]
    [SerializeField] Transform Grid;
    [SerializeField] GameObject ItemPrefab;

    List<ItemUI> ShopItems;

    #region Scroll Rect Variables
    [Header("Scrolling Variables")]
    [SerializeField] ScrollRect ScrollRect;
    [SerializeField] float spacing;
    [SerializeField] float height;

    float ScrollPercent;

    RectTransform GridRect;

    List<int> IndexesOnScreen;

    #endregion


    #region Built In/Setup functions
    private void Awake()
    {
        GridRect = Grid.GetComponent<RectTransform>();
        IndexesOnScreen = new List<int> { 0, 1, 2, 3 };
        PopulateGrid();
    }

    private void OnEnable()
    {
        ResetScrollRect();
    }


    private void PopulateGrid()
    {
        ShopItems = new List<ItemUI>();
        for(int i = 0; i < Items.Count; ++i)
        {
            GameObject NewItem = Instantiate(ItemPrefab, Grid);
            ItemUI ui = NewItem.GetComponent<ItemUI>();
            ui.SetupUI(Items[i].ItemName, Items[i].ItemCost, Items[i].ItemImage);
            ShopItems.Add(ui);
        }
        ScrollPercent = (spacing + height) / ((float)ShopItems.Count * (spacing + height));
        Debug.Log(ScrollPercent);
    }

    #endregion

    #region Scroll Rect functions

    void ResetScrollRect()
    {
        ScrollRect.verticalNormalizedPosition = 1;
    }

    void UpdateScrollRectPosition(int index)
    {
        if(IndexesOnScreen.Contains(index))
        {
            return;
        }

        if(index < IndexesOnScreen[0])
        {
            if (index == 0)
            {
                ScrollRect.verticalNormalizedPosition = 1f;
                IndexesOnScreen.Clear();
                IndexesOnScreen.AddRange(new int[] { 0, 1, 2, 3});
            }
            else
            {
                for (int i = 1; i < IndexesOnScreen.Count; ++i)
                {
                    IndexesOnScreen[i] = IndexesOnScreen[i - 1];
                }
                IndexesOnScreen[0] = index;

                ScrollRect.verticalNormalizedPosition += (ScrollPercent * 2f);
            }
        }
        else
        {
            if(index == ShopItems.Count - 1)
            {
                ScrollRect.verticalNormalizedPosition = 0f;
                IndexesOnScreen.Clear();
                IndexesOnScreen.AddRange(new int[] { ShopItems.Count - 4, ShopItems.Count - 3, ShopItems.Count - 2, ShopItems.Count - 1 });
            }
            else
            {
                for (int i = 0; i < IndexesOnScreen.Count - 1; ++i)
                {
                    IndexesOnScreen[i] = IndexesOnScreen[i + 1];
                }
                IndexesOnScreen[IndexesOnScreen.Count - 1] = index;

                ScrollRect.verticalNormalizedPosition -= (ScrollPercent * 2f);
            }
        }
    }


    #endregion

    #region Select/Deselect item functions

    public void SelectedItem(int index)
    {
        ShopItems[index].SelectItem();
        UpdateScrollRectPosition(index);
    }

    public void DeselectedItem(int index)
    {
        ShopItems[index].DeselectItem();
    }

    #endregion

    #region Get Attributes

    public int GetNumItems()
    {
        return Items.Count;
    }

    public GameObject GetPurchasedItem(int index)
    {
        return Items[index].ItemPrefab;
    }

    public int GetCostOfItem(int index)
    {
        return Items[index].ItemCost;
    }

    #endregion
}
