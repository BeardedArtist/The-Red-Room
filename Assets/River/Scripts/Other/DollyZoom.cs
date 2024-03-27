using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyZoom : MonoBehaviour
{
    private Camera camera;
    private float initialFrustumHeight, IniitialFov;

    public Transform target;

    public static DollyZoom Instance;

    public bool isZooming;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
    }

    public void Initialize(Transform target)
    {
        camera = GetComponent<Camera>();
        this.target = target;

        float DistanceFromTarget = Vector3.Distance(transform.position, target.position);
        initialFrustumHeight = ComputeFrustumHeight(DistanceFromTarget);
        IniitialFov = camera.fieldOfView;

        isZooming = true;
    }

    private float ComputeFrustumHeight(float distance)
    {
        return (2.0f * distance * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView));
    }

    private float ComputeFOV(float height, float distance)
    {
        return (2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg);
    }


    void Update()
    {
        if (isZooming)
        {
            float CurrentDistance = Vector3.Distance(transform.position, target.position);
            camera.fieldOfView = ComputeFOV(initialFrustumHeight, CurrentDistance);
        }
    }

    public void DeInitialize(){
        camera.fieldOfView = IniitialFov;
    }
}





