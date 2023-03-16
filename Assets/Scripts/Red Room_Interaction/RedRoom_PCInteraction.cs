using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class RedRoom_PCInteraction : MonoBehaviour
{
    // Object References
    [SerializeField] private GameObject computerUI;
    [SerializeField] private GameObject redRoomInteractUI;
    [SerializeField] private GameObject redRoomText;

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


                if (hasViewedOnce == true)
                {
                    for (int i = 0; i < objectsToDisappear.Length; i++)
                    {
                        objectsToDisappear[i].SetActive(false);
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

        // if (!FMODExtension.IsPlaying(redRoomComputerSFX))
        // {
        //     // redRoomComputerSFX = FMODUnity.RuntimeManager.CreateInstance(eventName);
        //     // redRoomComputerSFX.start();

        //     //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/RRCOMPSCREEN", GetComponent<Transform>().position);

        // }
    }

    // private void StopAudio()
    // {
    //     redRoomComputerSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     redRoomComputerSFX.release();
    // }

    // private void OnDestroy() 
    // {
    //     redRoomComputerSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     redRoomComputerSFX.release();
    // }
}
