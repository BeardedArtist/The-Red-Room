using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentFall : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private bool trig;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            trig = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            trig = false;
        }
    }


    private void Update() 
    {
        if (trig)
        {
            rb.useGravity = true;
        }    
    }
}
