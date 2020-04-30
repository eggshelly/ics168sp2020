using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableItem", menuName = "ScriptableItem")]
public class ScriptableItem: ScriptableObject
{
    public string ItemName;
    public Sprite ItemImage;
    public GameObject ItemPrefab;
    public int ItemCost;
}
