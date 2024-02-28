using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;
    [SerializeField] private SanityControler _sanityControler;
    [SerializeField] GameObject monsterCamera;
    [SerializeField] public bool isBlinking;
    [SerializeField] private float randomBlinkTimer;
    private float blinkTimer = 1.1f;
    private float everySecondTimer = 1f;

    void Start()
    {
        randomBlinkTimer = Random.Range(40.0f, 60.0f);
    }


    void Update()
    {
        randomBlinkTimer -= Time.deltaTime;

        if ((randomBlinkTimer <= 0 || Input.GetMouseButtonDown(1) && blinkTimer == 1.1f))
        {
            randomBlinkTimer = Random.Range(40.0f, 60.0f);
            StartBlink();
        }

        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;

            if (Input.GetMouseButton(1) && blinkTimer <= 0.55f)
            {
                blinkTimer += Time.deltaTime;
                everySecondTimer -= Time.deltaTime;

                blink_Anim.SetTrigger("Hold");
                blink_Anim_2.SetTrigger("Hold");
                
                monsterCamera.SetActive(true);

                if (everySecondTimer <= 0)
                {
                    _sanityControler.AlterSanity(0.5f);

                    everySecondTimer = 1f;
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                blink_Anim.ResetTrigger("Hold");
                blink_Anim_2.ResetTrigger("Hold");

                blink_Anim.SetTrigger("StopHold");
                blink_Anim_2.SetTrigger("StopHold");

                everySecondTimer = 1f;
            }
        }

        if (blinkTimer <= 0)
        {
            isBlinking = false;
            blinkTimer = 1.1f;

            blink_Anim.ResetTrigger("StopHold");
            blink_Anim_2.ResetTrigger("StopHold");

            monsterCamera.SetActive(false);
        }
    }


    void StartBlink()
    {
        isBlinking = true;

        blink_Anim.Play("TopLidBlink", 0, 0.25f);
        blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
    }
}
