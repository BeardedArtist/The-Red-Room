using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantGoHereTeleport : MonoBehaviour
{
    public Transform warpTarget;
    public Transform playerPosition;
    private bool trig = false;
    private bool hasAudioPlayed = false;
    public bool shouldPlaySFX;


    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerMovement playerMovement_Script;
    //[SerializeField] private GameObject warpPoint;
    [SerializeField] private GameObject player;

    
    [SerializeField] private GameObject playerCollider; // TEST


    // Animator Reference
    [SerializeField] private Animator blink_Anim = default;
    [SerializeField] private Animator blink_Anim_2 = default;


    private void Update() 
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            trig = true;
            
            if (shouldPlaySFX)
            {
                if (hasAudioPlayed == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Transitions/Transition_KO_TSUZUMI_NO JINGLE");
                    hasAudioPlayed = true;
                }
            }

            Vector3 offset = other.transform.position - transform.position;
            other.transform.position = warpTarget.position + offset;

            playerCollider.transform.position = playerPosition.position;

            // blink_Anim.Play("TopLidBlink", 0, 0.25f);
            // blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
            // StartCoroutine(TransitionAfterBlink());

            //StartCoroutine(TestTransition());
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;    
    }


    // IEnumerator TestTransition()
    // {
    //     //Debug.Log("Player Entered");

    //     //playerCollider.SetActive(false);
    //     //characterController.enabled = false;

    //     // yield return new WaitForSeconds(0.01f);
    //     // Vector3 offset = player.transform.position - transform.position;
    //     // player.transform.position = warpTarget.position + offset;
        
    //     //characterController.enabled = true;
    //     //playerCollider.SetActive(true);
    // }



    // IEnumerator TransitionAfterBlink()
    // {
    //     yield return new WaitForSeconds(0.40f);
        
    //     if (hasAudioPlayed == false)
    //     {
    //         FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Transitions/Transition_KO-TSUZUMI");
    //         hasAudioPlayed = true;
    //     }

    //     characterController.enabled = false;
    //     playerMovement_Script.enabled = false;
        
    //     player.transform.position = warpPoint.transform.position;
    //     player.transform.rotation = warpPoint.transform.rotation;

    //     characterController.enabled = true;
    //     playerMovement_Script.enabled = true;
    // }
}
