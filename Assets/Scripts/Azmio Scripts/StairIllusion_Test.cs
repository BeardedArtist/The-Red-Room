using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class StairIllusion_Test : MonoBehaviour
{
    [SerializeField] GameObject stairs;
    [SerializeField] MouseLook MouseLook;
    [SerializeField] Transform playerBody;
    Transform stairsTF;
    float currentRotationAngle = 0f;
    float RotationAngleOriginal = 0f;
    float RotationAngleNew = 90f;
    float RotationSpeed = 22.75f;
    bool isRotating = false;
    bool rotateDownNext = true;

    void Start()
    {
        stairsTF = stairs.GetComponent<Transform>();
    }

    void Update()
    {
        if (isRotating)
        {
            if (rotateDownNext)
            {
                RotateDown();
            }
            else
            {
                RotateUp();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRotating)
        {
            isRotating = true;
        }
    }

    void RotateDown()
    {
        float step = RotationSpeed * Time.deltaTime;

        stairsTF.Rotate(0f, 0f, step);
        MouseLook.xRotation += step;
        currentRotationAngle += step;
        
        if (currentRotationAngle >= RotationAngleNew)
        {
            isRotating = false;
            rotateDownNext = false;
        }
    }

    void RotateUp()
    {
        float step = RotationSpeed * Time.deltaTime;

        stairsTF.Rotate(0f, 0f, -step);
        MouseLook.xRotation -= step;
        currentRotationAngle -= step;
        
        if (currentRotationAngle <= RotationAngleOriginal)
        {
            isRotating = false;
            rotateDownNext = true;
        }
    }
}

