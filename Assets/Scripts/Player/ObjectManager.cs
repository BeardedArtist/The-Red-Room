using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject flashlightGO;
    [SerializeField] private GameObject CameraGO;
    [SerializeField] private Flashlight_Pickup _flashlight_Pickup;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) && _flashlight_Pickup._pickedUpFlashlight == true) // Make a test version that makes _pickedUpFlashlight == true on start
        {
            flashlightGO.SetActive(false);
            CameraGO.SetActive(true);
        }
    }
}
