using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MyBox;
using TMPro;
using System.Runtime.CompilerServices;

public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl, BookShelf, TestObject, Gizmo, Note, Door, ChoHan, ObjectRemoval, UncrouchOnTrigger }
    [Serializable]
    public struct Response
    {
        public string ResponseShort;
        public List<string> ResponseLong;
        public List<AudioClip> Responseclips;
        public bool StopCameraMovement;
        [Tooltip("Optional")] public Transform LookAtWhileTalking;
        [Range(1f, 10f)] public float EndDisableDelay;
    }

    [Serializable]
    public struct Details
    {
        public List<Details.DialogueElement> AllDialogueDetails;

        [Serializable]
        public struct DialogueElement
        {
            [TextArea(1, 5)] public string Dialogue;
            [Range(1f, 10f)] public float DisableDelay;
        }
        public List<AudioClip> DialogueAudio;
        public List<Response> Responses;
        public bool DisableAfterDialogue;
        [Range(1f, 10f)] public float EndDisableDelay;
        public bool StopPlayer;
        public bool StopCameraMovement;
        [Tooltip("Optional")] public Transform LookAtWhileTalking;
        [Range(1, 4)] public int Day;
    }


    #endregion

    public InteractableType type;

    [Foldout("Interactable Details", true)]
    [SerializeField]
    public List<Details> AllDetails;

    [SerializeField] public bool ActivatedOnTriggerCollision;
    [SerializeField] private bool DisableAfterTriggerCollision;
    [SerializeField, MyBox.Tag] private string CollisionWithTag;


    [Foldout("Debug", true)]
    [Range(0f, 10f)] public float GizmoSize;
    public Color GizmoColor;

    private float removeHoldTimer = 0;
    private float removeHoldLenght = 2f;
    private bool BookShelfInteracted = false;
    private bool FishBowlInteracted = false;


    [ButtonMethod]
    public void Interact()
    {
        Details OtherDetails = new Details();
        Details.DialogueElement DialogueDetails = new Details.DialogueElement();

        if (AllDetails.Count > 0) OtherDetails = AllDetails[0];
        //if (OtherDetails.AllDialogueDetails.Count > 0) DialogueDetails = OtherDetails.AllDialogueDetails[0];
        //DialogueDetails = OtherDetails.AllDialogueDetails;

        switch (type)
        {

            case InteractableType.Door:
                var animator = GetComponentInParent<Animator>();
                Debug.Log("Updating Door" + !animator.GetBool("Open"));
                animator.SetBool("Open", !animator.GetBool("Open"));
                DOVirtual.Float(0, 1, 0.1f, (value) => { }).OnComplete(() => { GetComponent<Collider>().enabled = true; });
                break;
            case InteractableType.BookShelf:
                DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, /*DialogueDetails.Dialogue, DialogueDetails.DisableDelay,*/ OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, false, null, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                BookShelfInteracted = true;
                break;
            case InteractableType.FishBowl:
                //DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, DialogueDetails.Dialogue, DialogueDetails.DisableDelay, OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, false, null, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                FishBowlInteracted = true;
                break;
            case InteractableType.ChoHan:
                //DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, DialogueDetails.Dialogue, DialogueDetails.DisableDelay, OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, false, null, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                break;
            case InteractableType.TestObject:
                //DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, DialogueDetails.Dialogue, DialogueDetails.DisableDelay, OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, true, OtherDetails.Responses, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                break;
            case InteractableType.ObjectRemoval:
                if (Input.GetKey(KeyCode.E))
                {
                    removeHoldTimer += Time.deltaTime;

                    if (removeHoldTimer >= removeHoldLenght)
                    {
                        gameObject.SetActive(false);
                        removeHoldTimer = 0;
                    }
                }
                break;
            case InteractableType.Gizmo:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (BookShelfInteracted || FishBowlInteracted)
        {
            StartCoroutine(StartMotherDialogue());
        }
    }


    public IEnumerator StartMotherDialogue()
    {
        Details DialogueDetails = new Details();
        if (AllDetails.Count > 0) DialogueDetails = AllDetails[0];

        yield return new WaitForSeconds(5);
        //DialogueManager.instance.ShowDialogue("Mother",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,true,DialogueDetails.MotherDialogueResponses,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, GizmoSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ActivatedOnTriggerCollision && other.gameObject.tag == CollisionWithTag)
        {
            switch (type)
            {
                case InteractableType.UncrouchOnTrigger:
                    if (other.gameObject.GetComponent<PlayerMovement>() != null)
                    {
                        other.gameObject.GetComponent<PlayerMovement>().ForceUncrouch();
                    }
                    break;
            }

            if (DisableAfterTriggerCollision)
            {
                gameObject.SetActive(false);
            }
        }


    }
}
