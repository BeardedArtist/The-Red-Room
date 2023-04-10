using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasBrush : MonoBehaviour
{
    [SerializeField] private GameObject KeyArtUI;
    [SerializeField] private GameObject ToBeContinuedUI;
    [SerializeField] private GameObject CreditsUI;

    // Player References
    [SerializeField] CharacterController characterController_Script;
    [SerializeField] MouseLook mouseLook_Script;

    private bool trig;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            trig = true;
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            trig = false;
        }
    }


    private void Update() 
    {
        if (trig)
        {
            StartCoroutine(ActivateEnding());
        }    
    }


    IEnumerator ActivateEnding()
    {
        characterController_Script.enabled = false;
        mouseLook_Script.enabled = false;

        KeyArtUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        KeyArtUI.SetActive(false);

        ToBeContinuedUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        ToBeContinuedUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        CreditsUI.SetActive(true);
    }
}
