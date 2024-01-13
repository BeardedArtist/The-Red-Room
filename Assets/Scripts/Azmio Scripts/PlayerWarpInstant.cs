using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarpInstant : MonoBehaviour
{
    [SerializeField] GameObject warpPlayerDestination;
    [SerializeField] GameObject player;
    CharacterController playerCharacterController;
    bool hasBeenTriggered = false;


    void Start()
    {
        playerCharacterController = player.GetComponent<CharacterController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && hasBeenTriggered == false)
        {
            StartCoroutine(DelayTransition());
        }
    }

    IEnumerator DelayTransition()
    {
        yield return new WaitForSeconds(0f);

        playerCharacterController.enabled = false;
        player.transform.position = warpPlayerDestination.transform.position;
        player.transform.rotation = warpPlayerDestination.transform.rotation;
        playerCharacterController.enabled = true;

        hasBeenTriggered = true;
    }
}
