using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    [SerializeField] GameObject warpPlayerDestination;
    [SerializeField] GameObject warpPlayerFinalDestination;
    [SerializeField] GameObject player;
    CharacterController playerCharacterController;

    [SerializeField] int loopAmount = 1;
    int loopNumber = 1;
    bool hasBeenTriggered = false;
    bool playerInTrigger = false;
    bool eKeyPressed = false;


    void Start()
    {
        playerCharacterController = player.GetComponent<CharacterController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            eKeyPressed = false;
        }
    }


    void Update()
    {
        if (playerInTrigger && Input.GetKey(KeyCode.E) && !hasBeenTriggered && !eKeyPressed)
        {
            eKeyPressed = true;

            if (loopNumber < loopAmount)
            {
                StartCoroutine(DelayTransition(warpPlayerDestination));
                loopNumber += 1;
            }

            else
            {
                StartCoroutine(DelayTransition(warpPlayerFinalDestination));
                hasBeenTriggered = true;
            }
        }
    }


    IEnumerator DelayTransition(GameObject warpPlayer)
    {
        yield return new WaitForSeconds(0.05f);

        playerCharacterController.enabled = false;
        player.transform.position = warpPlayer.transform.position;
        player.transform.rotation = warpPlayer.transform.rotation;
        playerCharacterController.enabled = true;
    }
}
