using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    [SerializeField] GameObject warpPlayerDestination;
    [SerializeField] GameObject player;
    CharacterController playerCharacterController;
    bool hasBeenTriggered = false;


    void Start()
    {
        playerCharacterController = player.GetComponent<CharacterController>();
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && hasBeenTriggered == false)
        {
            StartCoroutine(DelayTransition());
        }
    }

    IEnumerator DelayTransition()
    {
        yield return new WaitForSeconds(0.05f);

        playerCharacterController.enabled = false;
        player.transform.position = warpPlayerDestination.transform.position;
        player.transform.rotation = warpPlayerDestination.transform.rotation;
        playerCharacterController.enabled = true;

        hasBeenTriggered = true;
    }
}
