using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantGoHereTeleport : MonoBehaviour
{
    public Transform warpTarget;
    private bool trig = false;
    private bool hasAudioPlayed = false;


    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerMovement playerMovement_Script;
    [SerializeField] private GameObject warpPoint;
    [SerializeField] private GameObject player;

    
    [SerializeField] private GameObject playerCollider; // TEST


    // Animator Reference
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;



    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            trig = true;

            blink_Anim.Play("TopLidBlink", 0, 0.25f);
            blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
            StartCoroutine(TransitionAfterBlink());

            //StartCoroutine(TestTransition());
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;    
    }


    // IEnumerator TestTransition()
    // {
    //     Debug.Log("Player Entered");

    //     playerCollider.SetActive(false);
    //     characterController.enabled = false;
    //     yield return new WaitForSeconds(2f);
    //     Vector3 offset = player.transform.position - transform.position;
    //     player.transform.position = warpTarget.position + offset;
    //     characterController.enabled = true;
    //     playerCollider.SetActive(true);
    // }



    IEnumerator TransitionAfterBlink()
    {
        yield return new WaitForSeconds(0.40f);
        
        if (hasAudioPlayed == false)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Transitions/Transition_KO-TSUZUMI");
            hasAudioPlayed = true;
        }

        characterController.enabled = false;
        playerMovement_Script.enabled = false;
        
        player.transform.position = warpPoint.transform.position;
        player.transform.rotation = warpPoint.transform.rotation;

        characterController.enabled = true;
        playerMovement_Script.enabled = true;
    }
}
