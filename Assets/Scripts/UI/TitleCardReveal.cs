using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCardReveal : MonoBehaviour
{
    [SerializeField] private GameObject KeyArtUI;
    [SerializeField] private GameObject ToBeContinuedUI;
    [SerializeField] private GameObject CreditsUI;
    [SerializeField] private GameObject AI_Final;
    private bool trig;

    [SerializeField] CharacterController characterController_Script;
    [SerializeField] MouseLook mouseLook_Script;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Enemy"))
        {
            trig = true;
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
        AI_Final.SetActive(false);

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
