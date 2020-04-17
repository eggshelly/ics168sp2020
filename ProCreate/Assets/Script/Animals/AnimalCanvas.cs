using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCanvas : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] Canvas Canvas;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - new Vector3(this.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
    }
}
