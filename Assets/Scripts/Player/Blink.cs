using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private GameObject topLid;
    [SerializeField] private GameObject bottomLid;
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;
    [SerializeField] public bool isBlinking;
    [SerializeField] private float randomBlinkTimer;
    float blinkTimer = 1.1f;

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

                blink_Anim.SetTrigger("Hold");
                blink_Anim_2.SetTrigger("Hold");

                if (Input.GetMouseButtonUp(1)) //Animation doesn't reset once blinkHold is activarted + Blinkhold animation broken (Maybe Mouse up doesn't register)
                {
                    blink_Anim.ResetTrigger("Hold");
                    blink_Anim_2.ResetTrigger("Hold");
                    
                    blink_Anim.SetTrigger("StopHold");
                    blink_Anim_2.SetTrigger("StopHold");
                }
            }
            Debug.Log(blinkTimer);
        }

        if (blinkTimer <= 0)
        {
            isBlinking = false;
            blinkTimer = 1.1f;
        }
    }


    void StartBlink()
    {
        isBlinking = true;

        blink_Anim.Play("TopLidBlink", 0, 0.25f);
        blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
    }
}
