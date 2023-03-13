using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Notes : MonoBehaviour
{
    // Game Object References
    [SerializeField] private GameObject noteUI;
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] ObjectsToInteract;
    [SerializeField] private GameObject[] ObjectsToAppear;
    [SerializeField] private GameObject[] ObjectsToDisappear;

    // Bool References
    private bool trig;
    private bool isPickedUp = false;
    public bool isBathroomNotePickedUp = false;
    private bool isBloodyNotePickedUp = false;
    private bool isJournalPickedUp = false;

    // Script References
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private BloodAnimation bloodAnimation = default;
    [SerializeField] private Blink blink_Script;


    private void Start() 
    {
        characterController = player.GetComponent<CharacterController>();    
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Flashlight Eyes 2")
        {
            trig = true;
            pickUpUI.SetActive(true);
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;
        pickUpUI.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (trig == true && isPickedUp == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < ObjectsToInteract.Length; i++)
                {
                    if (ObjectsToInteract[i].name == "Journal")
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/JOURNALOPEN", GetComponent<Transform>().position);
                        noteUI.SetActive(true);
                        characterController.enabled = false;
                        mouseLook.mouseSensitivity = 0;
                        pickUpUI.SetActive(false);
                        isPickedUp = true;

                        blink_Script.enabled = false;
                        isJournalPickedUp = true;
                    }

                    if (ObjectsToInteract[i].name == "Note")
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/Page Grab_Updated", GetComponent<Transform>().position);
                        noteUI.SetActive(true);
                        characterController.enabled = false;
                        mouseLook.mouseSensitivity = 0;
                        pickUpUI.SetActive(false);
                        isPickedUp = true;
                    }

                    if (ObjectsToInteract[i].name == "Note_2" && isBathroomNotePickedUp == false)
                    {
                        NotePickUp();
                        isPickedUp = true;
                        isBathroomNotePickedUp = true;
                        // DISABLE MESH
                    }

                    if (ObjectsToInteract[i].name == "Note_3" && isBloodyNotePickedUp == false)
                    {
                        BathroomNotePickUp();
                        isBloodyNotePickedUp = true;
                    }
                }
            }
        }

        else if (trig == true && isPickedUp == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                noteUI.SetActive(false);
                characterController.enabled = true;
                mouseLook.enabled = true;
                mouseLook.mouseSensitivity = 3;
                isPickedUp = false;

                blink_Script.enabled = true;

                // isBloodyNotePickedUp = true;

                if (isJournalPickedUp == true)
                {
                    SetObjectsActive();
                }

                if (isBloodyNotePickedUp == true)
                {
                    bloodAnimation.BloodRiseAnimation_1_BathroomSink();
                    bloodAnimation.BloodRiseAnimation_2_BathroomRoom();
                }
            }
        }
    }


    public void NotePickUp()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/Page Grab_Updated", GetComponent<Transform>().position);
        noteUI.SetActive(true);
        characterController.enabled = false;
        mouseLook.mouseSensitivity = 0;
        pickUpUI.SetActive(false);
        isPickedUp = true;
    }

    public void BathroomNotePickUp()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/Page Grab_Updated", GetComponent<Transform>().position);
        noteUI.SetActive(true);
        characterController.enabled = false;
        mouseLook.mouseSensitivity = 0;
        pickUpUI.SetActive(false);
        isPickedUp = true;
    }



    private void SetObjectsActive()
    {
        for (int j = 0; j < ObjectsToAppear.Length; j++)
        {
            if (ObjectsToAppear[j] == null)
            {
                return;
            }
            else
            {
                ObjectsToAppear[j].SetActive(true);
            }
        }

        for (int j = 0; j < ObjectsToDisappear.Length; j++)
        {
            if (ObjectsToDisappear[j] == null)
            {
                return;
            }
            else
            {
                ObjectsToDisappear[j].SetActive(false);
            }
        }
    }
}
