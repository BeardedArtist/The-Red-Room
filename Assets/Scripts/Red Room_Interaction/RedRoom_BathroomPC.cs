using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRoom_BathroomPC : MonoBehaviour
{
    // UI References
    [SerializeField] private GameObject computerUI;
    [SerializeField] private GameObject redRoomInteractUI;
    [SerializeField] private GameObject redRoomText;

    // Light References
    [SerializeField] private GameObject flashingLight_1;
    [SerializeField] private GameObject flashingLight_2;

    // Script References
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Blink blink_Script;

    // Object List References
    [SerializeField] private GameObject[] objectsToAppear;
    [SerializeField] private GameObject[] objectsToDisappear;
    private bool hasViewedOnce = false;


    // FMOD Parameters ---------------------------
    // [SerializeField] EventReference eventName;
    // private static FMOD.Studio.EventInstance redRoomComputerSFX;

    [SerializeField] private AudioSource audioSource;
    private bool hasEdwardAudioPlayed = false;
    // FMOD Parameters ---------------------------

    private bool trig;
    private bool isViewingComputer = false;


    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Flashlight Eyes 2")
        {
            trig = true;
            redRoomInteractUI.SetActive(true);
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;
        redRoomInteractUI.SetActive(false);    
    }

    private void Update() 
    {
        PlayAudio();

        
        if (trig == true && isViewingComputer == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                computerUI.SetActive(true);
                redRoomText.SetActive(true);
                playerMovement.enabled = false;
                mouseLook.enabled = false;
                isViewingComputer = true;

                hasViewedOnce = true;

                blink_Script.enabled = false;
            }
        }

        else if (trig == true && isViewingComputer == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                computerUI.SetActive(false);
                redRoomText.SetActive(false);
                playerMovement.enabled = true;
                mouseLook.enabled = true;
                isViewingComputer = false;

                blink_Script.enabled = true;

                flashingLight_1.SetActive(false);
                flashingLight_2.SetActive(false);


                if (hasViewedOnce == true)
                {
                    for (int i = 0; i < objectsToDisappear.Length; i++)
                    {
                        objectsToDisappear[i].SetActive(false);
                    }

                    for (int i = 0; i < objectsToAppear.Length; i++)
                    {
                        objectsToAppear[i].SetActive(true);
                    }
                }
            }
        }    
    }


    private void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
