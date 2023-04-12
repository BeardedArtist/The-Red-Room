using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] Animator[] animator;
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
            for (int i = 0; i < animator.Length; i++)
            {
                animator[i].SetTrigger("Move");
            }
        }    
    }
}
