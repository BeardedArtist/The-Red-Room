using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestOnlyObjectRemove : MonoBehaviour
{
    [SerializeField] private GameObject pickUpUI;
    private bool trig;
    private float holdTimer;
    private float holdLenght = 2.5f;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Flashlight Eyes 2")
        {
            trig = true;
            pickUpUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        trig = false;
        pickUpUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (trig == true && Input.GetKey(KeyCode.Escape))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer == holdLenght)
            {
                gameObject.SetActive(false);
                holdTimer = 0;
            }
        }
        
    }
}
