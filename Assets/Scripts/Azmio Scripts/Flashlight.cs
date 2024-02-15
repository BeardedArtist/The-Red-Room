using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    [SerializeField] ChoHan _ChoHan;
    [SerializeField] TMP_Text spinResultText;
    public Light flashlight;
    public GameObject lightFlashAbility;

    public bool _lightIsOn;
    private float maxIntensity = 8f;
    private float intensityDecreaseRate = 0.05f;


    void Start()
    {
        flashlight = flashlight.GetComponent<Light>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spin();
        }

        if (Input.GetKeyDown(KeyCode.Q) && _lightIsOn)
        {
            StartCoroutine(FlashAbility());
        }

        if (_lightIsOn && flashlight.intensity >= 0f)
        {
            flashlight.intensity -= intensityDecreaseRate * Time.deltaTime;
        }
        else
        {
            _lightIsOn = false;
        }
    }


    void TurnOnFlashlight()
    {
        flashlight.intensity = maxIntensity;
        flashlight.enabled = true;
        _lightIsOn = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/FlashlightOnOff_Updated");
    }


    void Spin()
    {
        float spin = Random.Range(0f, 10f);

        if (spin >= 0f && spin < 1.5f) //Cho-Han
        {
            spinResultText.text = "Spin = Cho-Han";

            _ChoHan._rolledChoHan = true;
        }
        else if (spin >= 1.5f && spin < 2f) //Monster
        {
           spinResultText.text = "Spin = Monster";
        }
        else if (spin >= 2f && spin < 7f) //Light
        {
            spinResultText.text = "Spin = Light";

            TurnOnFlashlight();
        }
        else if (spin >= 7.5f && spin <= 10f) //Nothing
        {
            spinResultText.text = "Spin = Nothing";
        }
    }

    IEnumerator FlashAbility()
    {
        lightFlashAbility.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        lightFlashAbility.SetActive(false);

        //Add raycast -> monster logic here
    }
}
