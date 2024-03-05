using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MotherDialogue : MonoBehaviour
{
    //Why doesn't the variables show up in inspector and why can't Interactable references the coroutine in this script? Also not sure if dialogue will play properly
    //I tried to have it play in the interactable script but no dialogue was showning, I believe it's because the dialogue from the bookshelf interaction is trying to play.
    #region PreDefined
    public enum InteractableType { FishBowl, BookShelf, TestObject, Gizmo, Note, Door, ChoHan }
    [Serializable]
    public struct Response
    {
        public string ResponseShort;
        public List<string> ResponseLong;
        public List<AudioClip> Responseclips;
        public bool StopCameraMovement;
        [Tooltip("Optional")] public Transform LookAtWhileTalking;
        [Range(1f, 10f)] public float DisableDelay;
    }

    [Serializable]
    public struct Details
    {
        [TextArea(1, 5)] public List<string> Dialogue;
        public List<AudioClip> DialogueAudio;
        public List<Interactable.Response> MotherDialogueResponses;
        public bool DisableAfterDialogue;
        [Range(1f, 10f)] public float DisableDelay;
        public bool StopPlayer;
        public bool StopCameraMovement;
        [Tooltip("Optional")] public Transform LookAtWhileTalking;
        [Range(1, 4)] public int Day;
    }

    #endregion

    [Foldout("Interactable Details", true)]
    [SerializeField]
    public List<Details> AllDetails;

    public IEnumerator StartMotherDialogue()
    {
        Details DialogueDetails = new Details();
        if(AllDetails.Count>0) DialogueDetails = AllDetails[0];
        
        yield return new WaitForSeconds(5);
        DialogueManager.instance.ShowDialogue("Mother",DialogueDetails.Dialogue,DialogueDetails.DisableAfterDialogue,DialogueDetails.DisableDelay,DialogueDetails.DialogueAudio,true,DialogueDetails.MotherDialogueResponses,DialogueDetails.StopPlayer,DialogueDetails.StopCameraMovement,DialogueDetails.LookAtWhileTalking);
    }
}
