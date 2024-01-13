using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TemporaryPCScript : MonoBehaviour
{
    // Game Object References
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

    [SerializeField] private Material computerScreen;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float audioDelayTimer;
    [SerializeField] private GameObject Recording_2_Subtitles;
    private bool hasEdwardAudioPlayed = false;
    // FMOD Parameters ---------------------------

    private bool trig;
    private bool isViewingComputer = false;
    private bool hasViewedOnce = false;


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

                if (hasEdwardAudioPlayed == false)
                {
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/Voice Recordings/Edward Recording 1");
                    StartCoroutine(DelayVoiceAudio(audioDelayTimer));
                    hasEdwardAudioPlayed = true;
                }

                if (hasViewedOnce == true)
                {
                    computerScreen.SetColor("Computer Screen_RED", Color.black); // Currently not working

                    for (int i = 0; i < objectsToDisappear.Length; i++)
                    {
                        objectsToDisappear[i].SetActive(false);
                    }

                    for (int j = 0; j < objectsToAppear.Length; j++)
                    {
                        if (objectsToAppear[j] == null)
                        {
                            return;
                        }
                        else
                        {
                            objectsToAppear[j].SetActive(true);
                        }
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


    IEnumerator DelayVoiceAudio(float audioDelayTimer)
    {
        yield return new WaitForSeconds(audioDelayTimer);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Voice Recordings/Edward Recording 1");
        Recording_2_Subtitles.SetActive(true);
    }
}
