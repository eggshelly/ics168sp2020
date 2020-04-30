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

    RectTransform GridRect;

    #endregion


    #region Built In/Setup functions
    private void Awake()
    {
        GridRect = Grid.GetComponent<RectTransform>();
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
    }

    #endregion

    #region Scroll Rect functions

    void ResetScrollRect()
    {
        ScrollRect.verticalNormalizedPosition = 1;
    }

    void UpdateScrollRectPosition(int index)
    {

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
