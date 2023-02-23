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

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            trig = true;
            // Debug.Log("Player Entered");
            // Vector3 offset = other.transform.position - transform.position;
            // other.transform.position = warpTarget.position + offset;

            characterController.enabled = false;
            player.transform.position = warpPoint.transform.position;
            player.transform.rotation = warpPoint.transform.rotation;
            characterController.enabled = true;
        }
    }
}
