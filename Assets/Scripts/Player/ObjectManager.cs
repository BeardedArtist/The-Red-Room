using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private GameObject flashlightGO;
    [SerializeField] private GameObject CameraGO;
    [SerializeField] private Flashlight_Pickup _flashlight_Pickup;
    [SerializeField] private SanityControler _sanityControler;
    public bool _CameraEnabled;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))// && _flashlight_Pickup._pickedUpFlashlight == true) // Make sure to have condition to the <-- not commented when done testing
        {
            flashlightGO.SetActive(false);
            CameraGO.SetActive(true);
            _sanityControler._DecreaseSanity(5f);

            _CameraEnabled = true;
        }

        if(_CameraEnabled == false)
        {
            CameraGO.SetActive(false);
            flashlightGO.SetActive(true);
        }
    }
}
