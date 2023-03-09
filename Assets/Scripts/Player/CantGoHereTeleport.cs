using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantGoHereTeleport : MonoBehaviour
{
    public Transform warpTarget;
    private bool trig = false;


    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject warpPoint;
    [SerializeField] private GameObject player;

    //[SerializeField] private Collider playerCollider;


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


            // Debug.Log("Player Entered");
            // Vector3 offset = other.transform.position - transform.position;
            // other.transform.position = warpTarget.position + offset;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;    
    }


    // private void Update() 
    // {
    //     if (trig)
    //     {
 
    //     }    
    // }


    void StartBlinkAnimation()
    {
        blink_Anim.Play("TopLidBlink", 0, 0.25f);
        blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
    }

    IEnumerator TransitionAfterBlink()
    {
        yield return new WaitForSeconds(0.40f);
        
        characterController.enabled = false;

        player.transform.position = warpPoint.transform.position;
        player.transform.rotation = warpPoint.transform.rotation;

        characterController.enabled = true;
    }
}
