using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;

    public bool lightIsOn;

    void Start()
    {
        //lightIsOn = lightIsOn;
    }

    void Update()
    {
        if (lightIsOn == false)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashlight.SetActive(true);
                lightIsOn = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/FlashlightOnOff");
            }
        }

        else if (lightIsOn == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashlight.SetActive(false);
                lightIsOn = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/FlashlightOnOff");
            }
        }
    }
}
