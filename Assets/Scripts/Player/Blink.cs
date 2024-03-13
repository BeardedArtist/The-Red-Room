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
    [SerializeField] GameObject monsterCamera;
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

        if (Input.GetMouseButtonDown(1))
        {
            StartBlink(BlinkSpeed);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            EndBlink(BlinkSpeed);
        }

        #region Redundant
        //if ((randomBlinkTimer <= 0 || Input.GetMouseButtonDown(1) && blinkTimer == 1.1f))
        //{
        //    randomBlinkTimer = Random.Range(40.0f, 60.0f);
        //    StartBlink();
        //}

        //if (isBlinking)
        //{
        //    blinkTimer -= Time.deltaTime;

        //    if (Input.GetMouseButton(1) && blinkTimer <= 0.55f)
        //    {
        //        blinkTimer += Time.deltaTime;
        //        everySecondTimer -= Time.deltaTime;

        //        blink_Anim.SetTrigger("Hold");
        //        blink_Anim_2.SetTrigger("Hold");

        //        monsterCamera.SetActive(true);

        //        if (everySecondTimer <= 0)
        //        {
        //            _sanityControler.AlterSanity(0.5f);

        //            everySecondTimer = 1f;
        //        }
        //    }

        //    if (Input.GetMouseButtonUp(1))
        //    {
        //        blink_Anim.ResetTrigger("Hold");
        //        blink_Anim_2.ResetTrigger("Hold");

        //        blink_Anim.SetTrigger("StopHold");
        //        blink_Anim_2.SetTrigger("StopHold");

        //        everySecondTimer = 1f;
        //    }
        //}

        //if (blinkTimer <= 0)
        //{
        //    isBlinking = false;
        //    blinkTimer = 1.1f;

        //    blink_Anim.ResetTrigger("StopHold");
        //    blink_Anim_2.ResetTrigger("StopHold");

        //    monsterCamera.SetActive(false);
        //}
        #endregion
    }

    public void StartBlink(float _BlinkSpeed)
    {
        var blinkSequence = DOTween.Sequence();
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed));
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));
            
        var bottomLidSequence = DOTween.Sequence();
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed)).AppendCallback(() =>
        {
            ListIntractablesInView();
            if (ShowFlashingImageEnabled)
            {
                FlashingImage.SetActive(true);
            }
            else
            {
                monsterCamera.SetActive(true);
            }

            isBlinking = true;
        });
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));

    }

    public void EndBlink(float _BlinkSpeed)
    {
        var blinkSequence = DOTween.Sequence();
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed));
        blinkSequence.Append(TopLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));


        var bottomLidSequence = DOTween.Sequence();
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, Screen.height / 2), _BlinkSpeed)).AppendCallback(() =>
        {
            monsterCamera.SetActive(false);
            FlashingImage.SetActive(false);
            isBlinking = false;
        });
        bottomLidSequence.Append(BottomLid.rectTransform.DOSizeDelta(new Vector2(Screen.width, 0), _BlinkSpeed));

    }


    private void ListIntractablesInView()
    {
        var renderers = FindObjectsOfType<Renderer>();
        var cam = Camera.main;
        var renderersInView = renderers.Where(renderer => GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), renderer.bounds)).ToList();

        var AllInteractablesInView = renderersInView.Select(renderer => renderer.GetComponent<Interactable>()).Where(intractable => intractable != null).ToList();
        foreach (var intractable in AllInteractablesInView.Where(intractable => intractable.type == Interactable.InteractableType.ShiftOnBlink))
        {
            intractable.ShiftOnBlink();
        }
    }

   


    //void StartBlink()
    //{
    //    isBlinking = true;

    //    blink_Anim.Play("TopLidBlink", 0, 0.25f);
    //    blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
    //}
}
