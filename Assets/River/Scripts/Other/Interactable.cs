using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl,BookShelf,TestObject,Gizmo}
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
        var DialogueDetails = AllDetails[0];
        switch (type)
        {
            case InteractableType.BookShelf :
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,false,null,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
                break;
            case InteractableType.FishBowl :
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,false,null,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
                break;
            case InteractableType.TestObject:
                DialogueManager.instance.ShowDialogue("Player",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,true,DialogueDetails.Responses,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
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
