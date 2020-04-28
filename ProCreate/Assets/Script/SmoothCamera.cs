using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmoothCamera : MonoBehaviour
{
    #region Camera variables
    [Header("Look at targets")]
    public List<Transform> targets;
    [Header("Camera settings")]
    [SerializeField] Vector3 offset;
    [SerializeField] float easeFactor;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;
    [SerializeField] float zoomLimiter;
    private Vector3 velocity;
    private Camera camera;
    #endregion

    #region Built In Functions
    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        moveCamera();
        Debug.Log(GetGreatestDistance());
        zoomCamera();
    }

    private void zoomCamera()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, newZoom, Time.deltaTime);
    }
    #endregion

    #region Camera Methods
    private void moveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, easeFactor);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i<targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    public float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return Mathf.Max(bounds.size.x,bounds.size.z);
    }
    #endregion
}
