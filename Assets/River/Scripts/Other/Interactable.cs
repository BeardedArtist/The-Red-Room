using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MyBox;
using TMPro;
using System.Runtime.CompilerServices;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl, BookShelf, TestObject, Gizmo, Note, Door, ChoHan, ObjectRemoval, UncrouchOnTrigger, BathRoomReveal,ShiftOnBlink,FlashingImage,AiEnemyCrouch }
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

    [Foldout("Trigger Details", true)] [SerializeField]
    public bool DontTriggerByRaycast;
    [SerializeField] private bool ActivatedOnTriggerCollision;
    [SerializeField] private bool DisableAfterTriggerCollision;
    [SerializeField, MyBox.Tag] private string CollisionWithTag;

    [Foldout("AnimationDetails", true)]
    [SerializeField] private Animator interactable_animator;
    [SerializeField, Range(0f, 10f)] private float AnimationDelay;
    //[ConditionalField(nameof(type), false, InteractableType.BathRoomReveal)] public GameObject BathroomLight;

    [Foldout("Teleport Details", true)] 
    [SerializeField] private Vector3 TeleportPosition;

    [Foldout("Flashing Images", true)] 
    [SerializeField] private GameObject FlashingImage;

    [SerializeField, Range(0.1f, 10f)] private float BlinkTimer;


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
        var OtherDetails = new Details();
        //var DialogueDetails = new Details.DialogueElement();

        if (AllDetails.Count > 0) OtherDetails = AllDetails[0];
        //if (OtherDetails.AllDialogueDetails.Count > 0) DialogueDetails = OtherDetails.AllDialogueDetails[0];
        //DialogueDetails = OtherDetails.AllDialogueDetails; This line was causing errors for some unknown reason

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

        if (type != InteractableType.ShiftOnBlink) return;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(TeleportPosition, 0.6f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ActivatedOnTriggerCollision || other.gameObject.tag != CollisionWithTag) return;
        switch (type)
        {
            case InteractableType.UncrouchOnTrigger:
                if (other.gameObject.GetComponent<PlayerMovement>() != null)
                {
                    other.gameObject.GetComponent<PlayerMovement>().ForceUncrouch();
                }
                break;
            case InteractableType.BathRoomReveal:
                Camera.main.DOFarClipPlane(1000, 2f);
                RenderSettings.fog = false;
                DOVirtual.Float(0, 1, AnimationDelay, (value) => { }).OnComplete(() =>
                {
                    interactable_animator.SetTrigger("Activate");
                });
                break;
            case InteractableType.FlashingImage:
                Blink.instance.ShowFlashingImageEnabled = true;
                Blink.instance.FlashingImage = FlashingImage;
                Blink.instance.StartBlink(BlinkTimer);
                DOVirtual.Float(0, 1, Blink.instance.BlinkSpeed, (value) => { }).OnComplete(()=>{Blink.instance.EndBlink(Blink.instance.BlinkSpeed);});
                break;
            case InteractableType.AiEnemyCrouch:
                AI_StalkerController.instance.Crouch();
                break;
        }

        if (DisableAfterTriggerCollision)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!ActivatedOnTriggerCollision || other.gameObject.tag != CollisionWithTag) return;
        switch (type)
        {
            case InteractableType.AiEnemyCrouch:
                AI_StalkerController.instance.UnCrouch();
                break;
        }
    }

    public void ShiftOnBlink()
    {
        transform.position = TeleportPosition;
    }
}
