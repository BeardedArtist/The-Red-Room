using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;//Singleton
    public AudioSource DialogueSource;
    public TextAnimatorPlayer PlayerDialogue;
    public TextMeshProUGUI PlayerName;

    private bool DisableTextAfterShowing;
    private float DisableDelay;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance);
            instance = this;
        }
    }
    public void ShowDialogue(string Character,List<string> Dialogue,bool Disable,float DisableDelay,List<AudioClip> clip)
    {
        switch (Dialogue.Count)
        {
            case 1:
                PlayerName.text = Character + ": ";
                PlayerDialogue.ShowText( Dialogue[0]);
                if(clip.Count>0)PlayAudioClip(clip[0]);
                if (Disable) DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() => { PlayerDialogue.ShowText(""); PlayerName.text = ""; });
                break;
            
            case > 1:
                var index = 0;
                var nSeconds = 5; // Fading Timeout
                PlayerName.text = Character + ": ";
                PlayerDialogue.ShowText(Dialogue[index]);
                DOVirtual.Float(0, 1, nSeconds, (value) => { }).SetLoops(Dialogue.Count-1).OnStepComplete(()=>{
                    index++;
                    PlayerDialogue.ShowText(Dialogue[index]);
                }).OnComplete(() =>
                {
                    if (Disable) DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() => { PlayerDialogue.ShowText(""); PlayerName.text = ""; });
                });
                break;
            default:
                Debug.LogError("No Dialogue Entered");
                break;
        }
        
    }
    
    private void PlayAudioClip(AudioClip clip)
    {
        if (clip == null) return;
        DialogueSource.clip = clip;
        DialogueSource.Play();
    }
}


