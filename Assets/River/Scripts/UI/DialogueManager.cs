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
    [SerializeField] private PlayerWarp _playerWarp;

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
    //@Azmio Please Update this when your done with Dialogue manager
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
    public void ShowDialogue(string Character, List<Interactable.Details> AllDetails, bool HasResponses, int Responseindex)
    {
        var dayIndex = _playerWarp.loopNumber;
        var index = 0;
        PlayerName.text = Character + ": ";
        Interactable.Details IndexedDetails = AllDetails[dayIndex];

        if (IndexedDetails.StopPlayer)
        {
            playerMovement.walkSpeed = 0;
            playerMovement.canJump = false;
            playerMovement.canSprint = false;

            if (IndexedDetails.StopCameraMovement)
            {
                MouseLookScript.CanLook = false;
                if (IndexedDetails.LookAtWhileTalking != null)
                {
                    LookAtObjectCoroutine = StartCoroutine("LookAtTarget", IndexedDetails.LookAtWhileTalking);
                }
            }
        }

        PlayerDialogue.ShowText(IndexedDetails.AllDialogueDetails[index].Dialogue);
        if (IndexedDetails.DialogueAudio.Count > index) PlayAudioClip(IndexedDetails.DialogueAudio[index]);
        if (HasResponses && IndexedDetails.AllResponses.Count != 0) PlayerDialogue.ShowText(IndexedDetails.AllResponses[Responseindex].ResponseLong[index]);

        PlayNextDialogueTween();
        void PlayNextDialogueTween()
        {
            float DisableDelay = IndexedDetails.AllDialogueDetails[index].DisableDelay;
            DOVirtual.Float(0, 1, DisableDelay, (value) => { }).OnComplete(() =>
            {
                index++;
                if (index < IndexedDetails.AllDialogueDetails.Count || HasResponses && index <= IndexedDetails.AllDialogueDetails.Count)
                {
                    PlayerDialogue.ShowText(IndexedDetails.AllDialogueDetails[index].Dialogue);
                    if (IndexedDetails.DialogueAudio.Count > 0) PlayAudioClip(IndexedDetails.DialogueAudio[index]);
                    if (HasResponses && IndexedDetails.AllResponses.Count != 0) PlayerDialogue.ShowText(IndexedDetails.AllResponses[Responseindex].ResponseLong[index]);
                    PlayNextDialogueTween();
                }

                if (index >= IndexedDetails.AllDialogueDetails.Count)
                {
                    if (IndexedDetails.AllResponses.Count != 0)
                        ShowResponsePanel(AllDetails, IndexedDetails.AllResponses);

                    PlayerDialogue.ShowText("");
                    PlayerName.text = "";
                    playerMovement.walkSpeed = playerSpeed;
                    playerMovement.canJump = true;
                    playerMovement.canSprint = true;
                    MouseLookScript.CanLook = true;
                }
            });
        }
    }


    IEnumerator LookAtTarget(Transform target) //Doesn't work for MotherDialogue
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

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip == null) return;
        DialogueSource.clip = clip;
        DialogueSource.Play();
    }


    private void ShowResponsePanel(List<Interactable.Details> AllDetails, List<Interactable.Response> AllResponses) //Responses don't dissapear after appearing. Do we need "Disable After Dialogue" option? May not need stop player and look at options for individual responses either. "Stop Camera Movement" and "Look at" Functionality can be joined into one.
    {
        Debug.Log("Activating Response Panel");
        ResponsePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        for (var i = 0; i < AllResponses.Count; i++)
        {
            int Responseindex = i;
            Responses[Responseindex].SetActive(true);
            Responses[Responseindex].GetComponentInChildren<TextMeshProUGUI>().text = AllResponses[Responseindex].ResponseShort.ToString();
            Responses[Responseindex].GetComponent<Button>().onClick.AddListener(() =>
            {
                Cursor.lockState = CursorLockMode.Locked;
                ResponsePanel.SetActive(false);

                foreach (var responseButton in Responses)
                {
                    var Button = responseButton.GetComponent<Button>();
                    Button.onClick.RemoveAllListeners();
                    Button.gameObject.SetActive(false);
                }

                ShowDialogue("Player", AllDetails, true, Responseindex);
            });
        }
    }
}