using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGate : MonoBehaviour, Crossable, Interactable
{
    [Header("Fence Gate States")]
    [SerializeField] GameObject ClosedGate;
    [SerializeField] GameObject OpenedGate;
    [SerializeField] Directions OpenDirection;


    [SerializeField] bool Open = false;

    public bool IsOpen()
    {
        return Open;
    }
    
    public void Interact()
    {
        ChangeCrossableState();
    }

    void ChangeCrossableState()
    {
        if(Open)
        {
            Open = false;
            ClosedGate.SetActive(true);
            OpenedGate.SetActive(false);
        }
        else
        {
            Open = true;
            OpenedGate.SetActive(true);
            ClosedGate.SetActive(false);
        }
    }

    public GameObject GetParentStructure()
    {
        return this.transform.parent.gameObject;
    }
    
}
