using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject ObjectUI;


    /* void Update()
    {
        ObjectUI.SetActive(false);
    } */

    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectUI.SetActive(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ObjectUI.SetActive(false);
        }
    }
}
