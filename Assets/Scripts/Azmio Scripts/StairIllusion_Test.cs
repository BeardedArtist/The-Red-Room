using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class StairIllusion_Test : MonoBehaviour
{
    [SerializeField] GameObject stairs;
    Transform stairsTF;
    float zRotationAngle = 4.5f;
    bool hasRotated = false;

    void Start()
    {
        stairsTF = stairs.GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherGO = other.gameObject;

        if (other.tag == "Player" && !hasRotated)
        {
            StartCoroutine(IncrementalStairMovement());
            hasRotated = true;
        }
    }


    IEnumerator IncrementalStairMovement() //Make it so that Rotation is done incrementally every frame instead of through a for loop
    {
        WaitForSeconds incrementalStairMovementTime = new WaitForSeconds(0.1f);

        for (int i = 0; i < 10; i++)
        {
            stairsTF.Rotate(0f, 0f, zRotationAngle);
            yield return incrementalStairMovementTime;
        }
    }
}
