using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeRecorderPickup : MonoBehaviour
{
    // Objects & Bools that won't be touched
    [SerializeField] private bool Trig;
    [SerializeField] private GameObject goBackMessage;
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private MeshRenderer objectRender;
    [SerializeField] private Collider tapeRecorderCollider;
    public bool pickedUpTapeRecorder = false;
    public AudioSource audioSource;
    // Objects & Bools that won't be touched

    // Adding objects that will be effected
    // -------------------------------------------------------------------

    [SerializeField] private GameObject Part2Trigger;
    [SerializeField] private BoxCollider audioTrigger_Start;
    [SerializeField] private BoxCollider audioTrigger_Stop;
    [SerializeField] private GameObject flashingLights;
    [SerializeField] private float audioDelayTimer;
    [SerializeField] private GameObject Recording_1_Subtitles;
    private bool hasEdwardAudioPlayed = false;

    // Adding objects that will be effected
    // -------------------------------------------------------------------

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Flashlight Eyes 2")
        {
            Trig = true;
            InteractUI.SetActive(true);
        }    
    }
    private void OnTriggerExit(Collider other) 
    {
        Trig = false;    
        InteractUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Trig == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                audioSource.Stop();
                goBackMessage.SetActive(true);

                tapeRecorderCollider.enabled = false;
                Trig = false;
                InteractUI.SetActive(false);

                if (hasEdwardAudioPlayed == false)
                {
                    StartCoroutine(DelayVoiceAudio(audioDelayTimer));
                    hasEdwardAudioPlayed = true;
                }

                StartCoroutine(closeMessage());
                objectRender.enabled = false;
                Part2Trigger.SetActive(true);
                pickedUpTapeRecorder = true;

                audioTrigger_Start.enabled = false;
                audioTrigger_Stop.enabled = true;

                flashingLights.SetActive(true);
            }
        }
    }

    private IEnumerator closeMessage()
    {
        yield return new WaitForSeconds (5.0f);
        goBackMessage.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator DelayVoiceAudio(float audioDelayTimer)
    {
        yield return new WaitForSeconds(audioDelayTimer);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Voice Recordings/Edward Recording 1.1");
        Recording_1_Subtitles.SetActive(true);
    }
}
