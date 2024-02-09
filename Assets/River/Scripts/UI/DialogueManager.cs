using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Febucci.UI;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;//Singleton
    public AudioSource DialogueSource;
    public TextAnimatorPlayer PlayerDialogue;
    public TextMeshProUGUI PlayerName;

    public PlayerMovement playerMovement;
    public List<GameObject> Responses;
    public GameObject ResponsePanel;

    private bool DisableTextAfterShowing;
    private float DisableDelay;

    private float playerSpeed = 0;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    void Start()
    {
          playerSpeed = playerMovement.walkSpeed;
        ResponsePanel.SetActive(false);
    }
    public void ShowDialogue(string Character, List<string> Dialogue, bool Disable, float DisableDelay, List<AudioClip> clip, bool HasResponses, List<Interactable.Response> Responses,bool StopPlayer)
    {
        if(StopPlayer){
            playerMovement.walkSpeed = 0;
            Debug.Log("Stopping Player");
          
        }
        switch (Dialogue.Count)
        {
            case 1:
                PlayerName.text = Character + ": ";
                PlayerDialogue.ShowText(Dialogue[0]);
                if (clip.Count > 0) PlayAudioClip(clip[0]);
                if (Disable) DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() => { PlayerDialogue.ShowText(""); PlayerName.text = "";playerMovement.walkSpeed = playerSpeed; });
                if (HasResponses && Responses != null) ShowResponsePanel(Responses);
                break;

            case > 1:
                var index = 0;
                var nSeconds = 5; // Fading Timeout
                PlayerName.text = Character + ": ";
                PlayerDialogue.ShowText(Dialogue[index]);
                DOVirtual.Float(0, 1, nSeconds, (value) => { }).SetLoops(Dialogue.Count - 1).OnStepComplete(() =>
                {
                    index++;
                    PlayerDialogue.ShowText(Dialogue[index]);
                }).OnComplete(() =>
                {
                    if (HasResponses && Responses != null) ShowResponsePanel(Responses);
                    
                    if (Disable) DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() => { PlayerDialogue.ShowText(""); PlayerName.text = "";playerMovement.walkSpeed = playerSpeed; });
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

    private void ShowResponsePanel(List<Interactable.Response> AllResponses)
    {
        Debug.Log("Activating Response Panel");
        ResponsePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        for (int i = 0; i < AllResponses.Count; i++)
        {
            int index = i; // Create a local variable to capture the current value of i
            Responses[i].SetActive(true);
            Responses[i].GetComponentInChildren<TextMeshProUGUI>().text = AllResponses[i].ResponseShort.ToString();
            Responses[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                Cursor.lockState = CursorLockMode.Locked;
                ResponsePanel.SetActive(false);

                foreach (var responseButton in Responses)
                {
                    var Button = responseButton.GetComponent<Button>();
                    Button.onClick.RemoveAllListeners();
                    Button.gameObject.SetActive(false);
                }
                ShowDialogue("Player",
                    AllResponses[index].ResponseLong,
                    true,
                    AllResponses[index].DisableDelay,
                    AllResponses[index].Responseclips,
                    false,
                    null,
                    true);

            });
        }

    }

}


