using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakFlashlight : MonoBehaviour
{
    [SerializeField] Flashlight flashlight_Script;
    [SerializeField] GameObject flashlight_lightMain;
    [SerializeField] GameObject flashlight_Flicker;

    private bool hasBeenTriggered = false;


    public void DisableFlashlight()
    {
        if (hasBeenTriggered == false)
        {
            StartCoroutine(stopFlashlight());
            hasBeenTriggered = true;
        }
    }


    IEnumerator stopFlashlight()
    {
        flashlight_lightMain.SetActive(false);
        flashlight_Flicker.SetActive(true);

        yield return new WaitForSeconds(2f);

        flashlight_Flicker.SetActive(false);
        flashlight_Script._lightIsOn = false;

        flashlight_Script.enabled = false;
    }
}
