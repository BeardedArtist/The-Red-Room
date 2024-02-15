using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using MyBox;
public class PlayerWarp : MonoBehaviour
{
    #region Predefined
    [Serializable]
    public struct UpdatableGameobjects
    {
        public GameObject ObjectToUpdate;
        public bool Status;
    }
    
    #endregion
    [SerializeField] GameObject warpPlayerDestination;

    [SerializeField] GameObject player;
    CharacterController playerCharacterController;
    [SerializeField] bool isInstantWarp;
    [SerializeField] bool isWarpLoops;
    [SerializeField] int loopAmount = 1;
    [SerializeField] GameObject warpPlayerFinalDestination;
    int loopNumber = 1;
    bool hasBeenTriggered = false;
    bool playerInTrigger = false;
    bool eKeyPressed = false;

    [Foldout("Details", true)] 
    [SerializeField]public List<UpdatableGameobjects> ThingsToDisable;

    public bool EnableGameObjectsOnLoop;

    [FormerlySerializedAs("RotatePlayer")] public bool KeepPlayerRotation;

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

            if (!isWarpLoops)
            {
                StartCoroutine(DelayTransition(warpPlayerDestination));
                hasBeenTriggered = true;
            }

            else
            {
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

        else if (playerInTrigger && isInstantWarp)
        {
            StartCoroutine(DelayTransition(warpPlayerDestination));
        }
    }


    IEnumerator DelayTransition(GameObject warpPlayer)
    {
        float waitBeforeWarp = isInstantWarp ? 0f : 0.05f;

        yield return new WaitForSeconds(waitBeforeWarp);
        playerCharacterController.enabled = false;
        player.transform.position = warpPlayer.transform.position;
        if (!KeepPlayerRotation)
        {
            player.transform.rotation = warpPlayer.transform.rotation;
        }

       
        playerCharacterController.enabled = true;
        if (EnableGameObjectsOnLoop)
        {
            foreach (var Object in ThingsToDisable)
            {
                Object.ObjectToUpdate.SetActive(Object.Status);
            }
        }
    }
}
