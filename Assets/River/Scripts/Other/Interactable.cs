using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MyBox;
using TMPro;
using Febucci.UI;
using System.Runtime.CompilerServices;
using UnityEngine.Serialization;
using JetBrains.Annotations;

[SelectionBase]
public class Interactable : MonoBehaviour
{
    #region PreDefined
    // ReSharper disable once IdentifierTypo
    public enum InteractableType { FishBowl, BookShelf, TestObject, Gizmo, Note, Door, ChoHan, ObjectRemoval, UncrouchOnTrigger, Animation_BathRoomReveal, ShiftOnBlink, FlashingImage, AiEnemyCrouch, NE_Transition, ComputerScreen, Paper }
    public enum TransitionType { StaircaseToBedroom, HouseDoorToRoom, RoofToDataCenter }
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

    [ConditionalField(nameof(type), false, InteractableType.ChoHan)] [SerializeField] SanityControler _sanityControler;
    [ConditionalField(nameof(type), false, InteractableType.ChoHan)] [SerializeField] TMP_Text resultText, winLossText;
    [ConditionalField(nameof(type), false, InteractableType.ChoHan)] [SerializeField] GameObject betChoiceText;
    [ConditionalField(nameof(type), false, InteractableType.ChoHan)] [SerializeField] PlayerWarp _playerWarp;
    [Separator()]
    public InteractableType type;


    [Foldout("Dialogue Details", true)]
    [SerializeField]
    public List<Details> AllDetails;

    [Foldout("Trigger Details", true)]
    [SerializeField]
    public bool DontTriggerByRaycast;
    [SerializeField] private bool ActivatedOnTriggerCollision;
    [SerializeField] private bool DisableAfterTriggerCollision;
    [SerializeField, MyBox.Tag] private string CollisionWithTag;

    [Foldout("AnimationDetails", true)]
    [SerializeField] private Animator interactable_animator;
    [SerializeField, Range(0f, 10f)] private float AnimationDelay;

    [Foldout("Teleport Details", true)]
    [SerializeField] private Vector3 TeleportPosition;

    [Foldout("Flashing Images", true)]
    [SerializeField] private GameObject FlashingImage;
    [SerializeField, Range(0.1f, 10f)] private float BlinkTimer;

    [Foldout("Non Euclidean Transitions", true)]
    [SerializeField] private Transform GoToGameObject, TeleportToGameObject, TeleportPlayerBodyTo;
    [SerializeField] private Vector3 GoToGameObjectRotation, TeleportToGameObjectRotation;
    [SerializeField] private float GoToGameObjectTime;
    [SerializeField] private float WaitBeforeTeleportTime;

    [ConditionalField(nameof(transitionType), false, TransitionType.HouseDoorToRoom)][SerializeField] private string NEDoorToRoomTextOnMonitor;

    [SerializeField] private GameObject PlayerBody;
    [ConditionalField(nameof(transitionType), false, TransitionType.HouseDoorToRoom, TransitionType.StaircaseToBedroom)][SerializeField] private Interactable GoToComputerScreen;

    [ConditionalField(nameof(transitionType), false, TransitionType.RoofToDataCenter)][SerializeField] private Transform DollyZoomOutPosition, DollyZoomInPosition;
    [ConditionalField(nameof(transitionType), false, TransitionType.RoofToDataCenter)][SerializeField] private GameObject ScreenDataCenterImage, DataCenterSpawnCamera;
    [SerializeField] private TransitionType transitionType;

    [Foldout("Computer Screen", true)]
    [SerializeField] private TextAnimatorPlayer MonitorContent;
    [SerializeField] private string content_text;
    [SerializeField] private Transform ComputerScreenViewPosition;
    [SerializeField, Range(1f, 100f)] private float ExitTime;

    [Foldout("Common", true)]
    [SerializeField] private GameObject CutSceneCamera;

    [Foldout("Debug", true)]
    [Range(0f, 10f)] public float GizmoSize;
    public Color GizmoColor;

    private float removeHoldTimer = 0;
    private float removeHoldLenght = 2f;
    private bool BookShelfInteracted = false;
    private bool FishBowlInteracted = false;


    [ButtonMethod]
    public void Interact(bool choHanIsEven)
    {
        var OtherDetails = new Details();

        switch (type)
        {
            case InteractableType.Door:
                var animator = GetComponentInParent<Animator>();
                Debug.Log("Updating Door" + !animator.GetBool("Open"));
                animator.SetBool("Open", !animator.GetBool("Open"));
                DOVirtual.Float(0, 1, 0.1f, (value) => { }).OnComplete(() =>
                {
                    GetComponent<Collider>().enabled = true;
                });
                break;
            case InteractableType.BookShelf:
                DialogueManager.instance.ShowDialogue("Player", AllDetails, OtherDetails.DisableAfterDialogue, OtherDetails.DialogueAudio, false, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                BookShelfInteracted = true;
                break;
            case InteractableType.FishBowl:
                //DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, DialogueDetails.Dialogue, DialogueDetails.DisableDelay, OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, false, null, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);
                FishBowlInteracted = true;
                break;
            case InteractableType.ChoHan:
                //DialogueManager.instance.ShowDialogue("Player", OtherDetails.AllDialogueDetails, DialogueDetails.Dialogue, DialogueDetails.DisableDelay, OtherDetails.DisableAfterDialogue, OtherDetails.EndDisableDelay, OtherDetails.DialogueAudio, false, null, null, OtherDetails.StopPlayer, OtherDetails.StopCameraMovement, OtherDetails.LookAtWhileTalking);

                betChoiceText.SetActive(false);
                bool isDoubles = false;
                int diceRoll1 = UnityEngine.Random.Range(1, 7);
                int diceRoll2 = UnityEngine.Random.Range(1, 7);

                //Calculations
                int total = diceRoll1 + diceRoll2;
                bool resultIsEven = total % 2 == 0;
                resultText.text = total.ToString();

                if(_playerWarp.loopNumber == 3)
                {
                    if(choHanIsEven)
                    {
                        total = 8;
                        resultIsEven = true;
                    }

                    else if(!choHanIsEven)
                    {
                        total = 9;
                        resultIsEven = false;
                    }

                    resultText.text = total.ToString();
                }

                if (diceRoll1 == diceRoll2)
                {
                    isDoubles = true;
                    resultText.text = total.ToString() + " (Doubles)";
                }

                if (resultIsEven && choHanIsEven || !resultIsEven && !choHanIsEven) // Player wins
                {
                    winLossText.text = "You Picked Even = " + choHanIsEven + " And you Won!";

                    if (isDoubles)
                    {
                        _sanityControler.currentSanity += total * 2;
                    }
                    else
                    {
                        _sanityControler.currentSanity += total;
                    }
                }

                else // Player loses
                {
                    winLossText.text = "You Picked Even = " + choHanIsEven + " And you Lost!";
                }
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

            case InteractableType.ComputerScreen:
                CutSceneCamera.SetActive(true);
                PlayerMovement.instance.CanMove = false;
                MouseLook.instance.CanLook = false;
                var Sequence = DOTween.Sequence();
                Sequence.Append(CutSceneCamera.transform.DOMove(ComputerScreenViewPosition.transform.position, 1));
                Sequence.Append(DOVirtual.Float(0, 1, ExitTime, (value) => { })).OnComplete(() =>
                {
                    CutSceneCamera.transform.DOLocalMove(Vector3.zero, 1);
                    CutSceneCamera.transform.DOLocalRotate(Vector3.zero, 1).OnComplete(() =>
                    {
                        CutSceneCamera.SetActive(false);
                        PlayerMovement.instance.CanMove = true;
                        MouseLook.instance.CanLook = true;
                    });
                });

                if (MonitorContent != null) MonitorContent.ShowText(content_text);
                break;
            case InteractableType.NE_Transition:
                if (transitionType == TransitionType.StaircaseToBedroom)
                {
                    var Dooranimator = GetComponentInParent<Animator>();
                    Dooranimator.SetBool("Open", !Dooranimator.GetBool("Open"));
                    DOVirtual.Float(0, 1, 1f, (value) => { }).OnComplete(() =>
                    {
                        GetComponent<Collider>().enabled = true;
                        CutSceneCamera.SetActive(true);

                        PlayerMovement.instance.CanMove = false;
                        MouseLook.instance.CanLook = false;

                        var CameraPosition = CutSceneCamera.transform.position;
                        var Sequence = DOTween.Sequence();
                        Sequence.Append(PlayerBody.transform.DOMove(TeleportPlayerBodyTo.position, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(CameraPosition, 0));
                        Sequence.Append(PlayerBody.transform.DOMove(TeleportPlayerBodyTo.position, 0));
                        Sequence.Append(CutSceneCamera.transform.DORotate(GoToGameObjectRotation, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(GoToGameObject.transform.position, GoToGameObjectTime));
                        Sequence.Append(DOVirtual.Float(0, 1, WaitBeforeTeleportTime, (value) => { }));
                        Sequence.Append(CutSceneCamera.transform.DORotate(TeleportToGameObjectRotation, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(TeleportToGameObject.transform.position, 0))
                            .OnComplete(() => { GoToComputerScreen.Interact(false); });
                    });
                }

                else if (transitionType == TransitionType.HouseDoorToRoom)
                {
                    // var Dooranimator = GetComponentInParent<Animator>();
                    // Dooranimator.SetBool("Open", !Dooranimator.GetBool("Open"));
                    DOVirtual.Float(0, 1, 1f, (value) => { }).OnComplete(() =>
                    {
                        //GetComponent<Collider>().enabled = true;
                        CutSceneCamera.SetActive(true);

                        PlayerMovement.instance.CanMove = false;
                        MouseLook.instance.CanLook = false;

                        var CameraPosition = CutSceneCamera.transform.position;
                        var Sequence = DOTween.Sequence();
                        Sequence.Append(PlayerBody.transform.DOMove(TeleportPlayerBodyTo.position, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(CameraPosition, 0));
                        Sequence.Append(PlayerBody.transform.DOMove(TeleportPlayerBodyTo.position, 0));
                        Sequence.Append(CutSceneCamera.transform.DORotate(GoToGameObjectRotation, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(GoToGameObject.transform.position, GoToGameObjectTime));
                        Sequence.Append(DOVirtual.Float(0, 1, WaitBeforeTeleportTime, (value) => { }));
                        Sequence.Append(CutSceneCamera.transform.DORotate(TeleportToGameObjectRotation, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(TeleportToGameObject.transform.position, 0))
                            .OnComplete(() =>
                            {
                                GoToComputerScreen.content_text = NEDoorToRoomTextOnMonitor;
                                GoToComputerScreen.Interact(false);
                            });
                    });
                }

                else if (transitionType == TransitionType.RoofToDataCenter)
                {
                    DOVirtual.Float(0, 1, 1f, (value) => { }).OnComplete(() =>
                    {
                        GetComponent<Collider>().enabled = false;
                        CutSceneCamera.SetActive(true);

                        PlayerMovement.instance.CanMove = false;
                        MouseLook.instance.CanLook = false;



                        var CameraPosition = CutSceneCamera.transform.position;
                        var Sequence = DOTween.Sequence();
                        Sequence.Append(PlayerBody.transform.DOMove(TeleportPlayerBodyTo.position, 0));
                        Sequence.Append(PlayerBody.transform.DORotate(TeleportPlayerBodyTo.transform.rotation.eulerAngles, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(CameraPosition, 0));
                        Sequence.Append(CutSceneCamera.transform.DORotate(GoToGameObjectRotation, 0));
                        Sequence.Append(CutSceneCamera.transform.DOMove(GoToGameObject.transform.position, GoToGameObjectTime)).AppendCallback(() =>
                        {
                            CutSceneCamera.GetComponent<DollyZoom>().Initialize(gameObject.transform);
                            ScreenDataCenterImage.SetActive(true);
                            DataCenterSpawnCamera.SetActive(true);
                        });
                        //Sequence.Append(CutSceneCamera.transform.DOMove(GoToGameObject.transform.position, GoToGameObjectTime)); //Run Some Monitor Text or whatever at this point here
                        Sequence.Append(CutSceneCamera.transform.DOMove(DollyZoomOutPosition.transform.position, 1));
                        Sequence.Append(CutSceneCamera.transform.DOMove(DollyZoomInPosition.transform.position, 2)).OnComplete(() =>
                        {
                            CutSceneCamera.GetComponent<DollyZoom>().DeInitialize();
                            CutSceneCamera.SetActive(false);
                            PlayerMovement.instance.CanMove = true;
                            MouseLook.instance.CanLook = true;
                            DataCenterSpawnCamera.SetActive(false);//Saving Resources
                        });

                    });
                }


                break;


                // default:
                // throw new ArgumentOutOfRangeException();
        }

        if (BookShelfInteracted || FishBowlInteracted)
        {
            StartCoroutine(StartMotherDialogue());
        }
    }


    public IEnumerator StartMotherDialogue()
    {
        var DialogueDetails = new Details();
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
            case InteractableType.Animation_BathRoomReveal:
                Camera.main.DOFarClipPlane(1000, 2f);
                RenderSettings.fog = false;
                DOVirtual.Float(0, 1, AnimationDelay, (value) => { }).OnComplete(() =>
                {
                    interactable_animator.SetTrigger("Activate");
                });
                break;
            case InteractableType.FlashingImage:
                GetComponent<Collider>().enabled = false;
                Blink.instance.ShowFlashingImageEnabled = true;
                Blink.instance.FlashingImage = FlashingImage;
                Blink.instance.CloseEyesForcibly(BlinkTimer);
                DOVirtual.Float(0, 1, BlinkTimer + 1, (value) => { }).OnComplete(() =>
                  {
                      Blink.instance.OpenEyesForcibly(Blink.instance.BlinkSpeed);
                      DOVirtual.Float(0, 1, BlinkTimer + 1, (value) => { }).OnComplete(() =>
                      {
                          GetComponent<Collider>().enabled = true;
                      });
                  });
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

    private void OnCollisionEnter(Collision other)
    {
        if (type == InteractableType.Paper)
        {
            Debug.Log(other.gameObject.name);
        }
    }
}
