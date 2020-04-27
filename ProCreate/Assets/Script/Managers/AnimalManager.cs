using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager instance = null;

    [Header("Objects Needed By Animals")]
    [SerializeField] GameObject CanvasPrefab;
    


    #region BuildInFunctions

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion


    #region Functions to Get Objects

    public GameObject GetCanvasPrefab()
    {
        return CanvasPrefab;
    }

    #endregion
}
