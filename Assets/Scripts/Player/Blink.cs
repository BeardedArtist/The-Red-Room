using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;

public class Blink : MonoBehaviour
{
    //[SerializeField] private Animator blink_Anim;
    //[SerializeField] private Animator blink_Anim_2;
    [SerializeField] private SanityControler _sanityControler;
    [SerializeField] private GameObject monsterCamera;
    [SerializeField] public bool isBlinking;
    [SerializeField] private float randomBlinkTimer;
    //private float blinkTimer = 1.1f;
    private float everySecondTimer = 1f;

    [SerializeField] private Image TopLid, BottomLid;

    [SerializeField, Range(0.1f, 2f)] public float BlinkSpeed = 1f;

    public static Blink instance;

    [ReadOnly]public bool ShowFlashingImageEnabled;
    [ReadOnly] public GameObject FlashingImage;
    
    private void Start()
    {
        randomBlinkTimer = Random.Range(40.0f, 60.0f);
        instance = this;
    }

    private void Update()
    {
        randomBlinkTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) || randomBlinkTimer <= 0)
        {
            randomBlinkTimer = Random.Range(40.0f, 60.0f);
            isBlinking = true;
    
            DOTween.Sequence().Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), BlinkSpeed));
            DOTween.Sequence().Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), BlinkSpeed)).AppendCallback(() =>
            {
                ListInteractablesInView();
            });
        }

        if (Input.GetMouseButtonUp(1))
        {
            isBlinking = false;
            if(FlashingImage != null)
            FlashingImage.SetActive(false);

            DOTween.Sequence().Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), BlinkSpeed));
            DOTween.Sequence().Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), BlinkSpeed));
        }

        else if (Input.GetMouseButtonDown(1) && monsterCamera.activeSelf == true)
        {
            PlayerToMonsterView(BlinkSpeed);
        }

        else if (Input.GetMouseButtonUp(1) && monsterCamera.activeSelf == true)
        {
            MonsterToPlayerView(BlinkSpeed);
        }


        if (isBlinking)
        {
            everySecondTimer -= Time.deltaTime;

            if (everySecondTimer <= 0)
            {
                _sanityControler.AlterSanity(0.5f);
                everySecondTimer = 1f;
            }
        }
    }

    private void PlayerToMonsterView(float _BlinkSpeed)
    {
        isBlinking = true;
        var blinkSequence = DOTween.Sequence();
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed));
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));
            
        var bottomLidSequence = DOTween.Sequence();
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed)).AppendCallback(() =>
        {
            ListInteractablesInView();
        });
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));
    }

    private void MonsterToPlayerView(float _BlinkSpeed)
    {
        isBlinking = false;
        if(FlashingImage != null)
            FlashingImage.SetActive(false);
            
        var blinkSequence = DOTween.Sequence();
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed));
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));


        var bottomLidSequence = DOTween.Sequence();
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed)).AppendCallback(() =>
        {
            monsterCamera.SetActive(false);
        });
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));

    }

    public void CloseEyesForcibly(float _BlinkSpeed)
    {
        TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed).SetEase(Ease.Linear);
        BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed).SetEase(Ease.Linear);

        DOVirtual.Float(0, 1, _BlinkSpeed, (value) =>
        {
            if (value > 0.9)
            {
                if (ShowFlashingImageEnabled)
                {
                    FlashingImage.SetActive(true);
                }
            }
         
        }).SetEase(Ease.Linear);

    }
    public void OpenEyesForcibly(float _BlinkSpeed)
    {
        FlashingImage.SetActive(false);
        TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed);
        BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed);
    }


    // ReSharper disable once IdentifierTypo
    private static void ListInteractablesInView()
    {
        var renderers = FindObjectsOfType<Renderer>();
        var cam = Camera.main;
        var renderersInView = renderers.Where(renderer => GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), renderer.bounds)).ToList();

        var AllInteractablesInView = renderersInView.Select(renderer => renderer.GetComponent<Interactable>()).Where(interactable => interactable != null).ToList();
        foreach (var interactable in AllInteractablesInView.Where(interactable => interactable.type == Interactable.InteractableType.ShiftOnBlink))
        {
            interactable.ShiftOnBlink();
        }
    }
}
