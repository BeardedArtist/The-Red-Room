using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MyBox;
using TMPro;

public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl,BookShelf,TestObject,Gizmo,Note,Door}
    [Serializable]
    public struct Response{
        public string ResponseShort;
        public List<string> ResponseLong;
        public List<AudioClip> Responseclips;
        public bool StopCameraMovement;
        [Tooltip("Optional")]public Transform LookAtWhileTalking;
        [Range(1f, 10f)] public float DisableDelay;
    }
    
    [Serializable]
    public struct Details
    {
        [TextArea(1,5)]public List<string> Dialogue;
        public List<AudioClip> DialogueAudio;
        public List<Response> Responses;
        public bool DisableAfterDialogue;
        [Range(1f, 10f)] public float DisableDelay;
        public bool StopPlayer;
        public bool StopCameraMovement;
        [Tooltip("Optional")]public Transform LookAtWhileTalking;
        [Range(1, 4)] public int Day;
    }

   
    #endregion
    
    public InteractableType type;

    [Foldout("Interactable Details", true)] [SerializeField]
    public List<Details> AllDetails;

    [Foldout("Debug",true)]
    [Range(0f, 10f)] public float GizmoSize;
    public Color GizmoColor;

   
    [ButtonMethod]
    public void Interact()
    {
        
        Details DialogueDetails = new Details();
            
        if(AllDetails.Count>0) DialogueDetails = AllDetails[0];
        switch (type)
        {
            
            case InteractableType.Door :
                var animator = GetComponentInParent<Animator>();
                Debug.Log("Updating Door"+ !animator.GetBool("Open"));
                animator.SetBool("Open",!animator.GetBool("Open"));
                DOVirtual.Float(0, 1, 0.1f, (value) => { }).OnComplete(() => { GetComponent<Collider>().enabled = true; });
                break;
            case InteractableType.BookShelf :
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,false,null,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
                break;
            case InteractableType.FishBowl :
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,false,null,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
                break;
            case InteractableType.TestObject:
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,true,DialogueDetails.Responses,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
                break;
            case InteractableType.Gizmo:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, GizmoSize);
    }
}
