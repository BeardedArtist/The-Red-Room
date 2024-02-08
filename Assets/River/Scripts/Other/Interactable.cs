using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class Interactable : MonoBehaviour
{
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl,BookShelf }

    [Serializable]
    public struct Details
    {
        public string Dialogue;
        [Range(1, 3)] public int Day;
        public AudioClip DialogueAudio;
    }

    public InteractableType type;

    [Foldout("Interactable Details", true)] [SerializeField]
    public List<Details> AllDetails;


    public void Interact()
    {
        
    }
}
