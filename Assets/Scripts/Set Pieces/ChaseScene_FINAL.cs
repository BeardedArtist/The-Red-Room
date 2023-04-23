using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseScene_FINAL : MonoBehaviour
{
    // AI Reference
    [SerializeField] private GameObject AI_Chase;
    [SerializeField] private GameObject[] DoorCovers;
    
    // Script References
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouseLook;

    // bool Reference
    [SerializeField] private bool trig;
    [SerializeField] private bool isAnimationPlaying = false;

    // Animator Reference
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;
    [SerializeField] private Blink blink_Script;


    // COMMENTED OUT CODE - IMPLEMENT LATER MAYBE -----------------------------------------------

    //Animator Reference
    //This was the animator for the player body (Camera Control Cutscene) -- COMMENTED OUT NOW
    [SerializeField] Animator animator;

    [SerializeField] private GameObject cutSceneCamera;
    private bool hasAnimationPlayed = false;
    private bool hasAudioPlayed = false;

    // COMMENTED OUT CODE - IMPLEMENT LATER MAYBE -----------------------------------------------



    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            trig = true;
            blink_Anim.Play("TopLidBlink", 0, 0.25f);
            blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
            //HandleFinalScene();
            HandlePlayerCamera();
        }
    }

    void HandleFinalScene()
    {
        StartCoroutine(ActivateFinalScene());
    }

    IEnumerator ActivateFinalScene()
    {
        yield return new WaitForSeconds(0.40f);
        AI_Chase.SetActive(true);

        // Activate New Environment
    }


    void HandlePlayerCamera()
    {
        if (hasAnimationPlayed == false)
        {
            //StartCoroutine(PositionPlayerForAnimation());
            StartCoroutine(startAnimation());
            StartCoroutine(startChaseAudio());
        }

        StartCoroutine(HandleManualBlink());
    }

    IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(0.40f);
        animator.SetBool("FinalChaseSceneAnimTrigger", true);
        isAnimationPlaying = true;

        if (isAnimationPlaying == true)
        {
            cutSceneCamera.SetActive(true);
            playerMovement.enabled = false;
            mouseLook.enabled = false;
        }

        yield return new WaitForSeconds(14.7f);
        cutSceneCamera.SetActive(false);
        isAnimationPlaying = false;
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        trig = false;

        hasAnimationPlayed = true;
    }


    IEnumerator startChaseAudio()
    {
        yield return new WaitForSeconds(14.7f);

        if (hasAudioPlayed == false)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Ideas/Bathroom_JumpScareIdea_3");
            hasAudioPlayed = true;

            blink_Script.enabled = false;
        }
    }

    IEnumerator HandleManualBlink()
    {
        yield return new WaitForSeconds(14f);
        blink_Anim.Play("TopLidBlink", 0, 0.15f);
        blink_Anim_2.Play("BottomLidBlink", 0, 0.15f);
    }
}
