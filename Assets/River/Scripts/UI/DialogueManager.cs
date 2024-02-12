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
    public static DialogueManager instance; //Singleton
    public AudioSource DialogueSource;
    public TextAnimatorPlayer PlayerDialogue;
    public TextMeshProUGUI PlayerName;

    public PlayerMovement playerMovement;
    public MouseLook MouseLookScript;

    public Transform Player, Camera;
    
    public List<GameObject> Responses;
    public GameObject ResponsePanel;

    private bool DisableTextAfterShowing;
    private float DisableDelay;

    private float playerSpeed = 0;

    private Coroutine LookAtObjectCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    private void Start()
    {
        playerSpeed = playerMovement.walkSpeed;
        ResponsePanel.SetActive(false);
    }

    /// <summary>
    /// A function that can be called from any script using `DialogueManager.instance.ShowDialogue` to start a conversation with other characters or oneself
    /// </summary>
    /// <param name="Character">Name of the character that's speaking the dialogue</param>
    /// <param name="Dialogue">the script the character will follow</param>
    /// <param name="Disable">Disable the subtitles after the character is done speaking</param>
    /// <param name="DisableDelay">How long does it take before the subtitles are disabled</param>
    /// <param name="clip">AudioClip to accompany the subtitles , can be left null</param>
    /// <param name="HasResponses">Optional responses to the words spoken by the given character</param>
    /// <param name="Responses">Response script followed by the other character</param>
    /// <param name="StopPlayer">Stop movement when character is speaking</param>
    /// <param name="StopCameraMovement">Stop Camera panning when the character is speaking</param>
    /// <param name="LookAT">Look at a specific object while the camera is locked</param>
    public void ShowDialogue(string Character, List<string> Dialogue, bool Disable, float DisableDelay,
        List<AudioClip> clip, bool HasResponses, List<Interactable.Response> Responses, bool StopPlayer,
        bool StopCameraMovement, Transform LookAT)
    {
        if (StopPlayer)
        {
            playerMovement.walkSpeed = 0;
            if (StopCameraMovement)
            {
                MouseLookScript.CanLook = false;
                if (LookAT != null)
                {
                    LookAtObjectCoroutine = StartCoroutine("LookAtTarget", LookAT);
                }
            }
        }

        switch (Dialogue.Count)
        {
            case 1:
                PlayerName.text = Character + ": ";
                PlayerDialogue.ShowText(Dialogue[0]);
                if (clip.Count > 0) PlayAudioClip(clip[0]);
                if (Disable)
                    DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() =>
                    {
                        PlayerDialogue.ShowText("");
                        PlayerName.text = "";
                        playerMovement.walkSpeed = playerSpeed;
                        MouseLookScript.CanLook = true;
                    });
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

                    if (Disable)
                        DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() =>
                        {
                            PlayerDialogue.ShowText("");
                            PlayerName.text = "";
                            playerMovement.walkSpeed = playerSpeed;
                            MouseLookScript.CanLook = true;
                        });
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

        for (var i = 0; i < AllResponses.Count; i++)
        {
            var index = i; // Create a local variable to capture the current value of i
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
                    true,
                    AllResponses[index].StopCameraMovement,
                    AllResponses[index].LookAtWhileTalking);
            });
        }
    }

    IEnumerator LookAtTarget(Transform target)
    {
        while (true)
        {
            var cameraDirection = Camera.forward;
            var directionToTarget = (target.position - Camera.position).normalized;

            if (Vector3.Dot(cameraDirection, directionToTarget) > 0.99f)
            {
                StopCoroutine(LookAtObjectCoroutine);
                yield break; 
            }

            var targetRotation = Quaternion.LookRotation(directionToTarget);
            // Apply Y rotation to Player
            Player.rotation = Quaternion.Slerp(Player.rotation, Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), Time.deltaTime);
            // Apply X rotation to Camera
            Camera.rotation = Quaternion.Slerp(Camera.rotation, Quaternion.Euler(targetRotation.eulerAngles.x, Camera.eulerAngles.y, 0), Time.deltaTime);
            
            yield return null;
        }
    }
}