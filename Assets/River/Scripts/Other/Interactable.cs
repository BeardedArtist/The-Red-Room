using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl,BookShelf }
    
    [Serializable]
    public struct Details
    {
        [TextArea(1,5)]public List<string> Dialogue;
        public List<AudioClip> DialogueAudio;
        public List<string> Responses;
        public List<AudioClip> ResponseClips;
        public bool DisableAfterDialogue;
        [Range(1f, 10f)] public float DisableDelay;
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
        switch (type)
        {
            case InteractableType.BookShelf :
                DialogueManager.instance.ShowDialogue("Player",AllDetails[0].Dialogue,AllDetails[0].DisableAfterDialogue,AllDetails[0].DisableDelay,AllDetails[0].DialogueAudio);
                break;
            case InteractableType.FishBowl :
                DialogueManager.instance.ShowDialogue("Player",AllDetails[0].Dialogue,AllDetails[0].DisableAfterDialogue,AllDetails[0].DisableDelay,AllDetails[0].DialogueAudio);
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
