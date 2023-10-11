using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject flashlightGO;
    [SerializeField] private GameObject CameraGO;
    [SerializeField] private Flashlight_Pickup _flashlight_Pickup;
    public bool _CameraEnabled = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))// && _flashlight_Pickup._pickedUpFlashlight == true) // Make sure to have flashlight pick up check when done testing
        {
            flashlightGO.SetActive(false);
            CameraGO.SetActive(true);

            _CameraEnabled = true;
        }

        if(_CameraEnabled == false) //Only want this to play when Camera is put away
        {
            CameraGO.SetActive(false);
            flashlightGO.SetActive(true);
        }
    }
}
