using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTeleport : MonoBehaviour
{
    // Player References
    [SerializeField] private GameObject player;
    private CharacterController characterController;

    // Place player will warp to
    [SerializeField] private GameObject warpTarget;

    // Blink Animation
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;


    private void Start() 
    {
        characterController = player.GetComponent<CharacterController>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            blink_Anim.Play("TopLidBlink", 0, 0);
            blink_Anim_2.Play("BottomLidBlink", 0, 0);

            // Play Transition Audio

            StartCoroutine(TeleportPlayer());
        }    
    }

    private IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(0.7f);
        characterController.enabled = false;
        player.transform.position = warpTarget.transform.position;
        player.transform.rotation = warpTarget.transform.rotation;
        characterController.enabled = true;
    }
}
