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

        if ((randomBlinkTimer <= 0 || Input.GetMouseButton(1) && blinkTimer == 1.1f))
        {
            randomBlinkTimer = Random.Range(40.0f, 60.0f);
            StartBlink();
        }

        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
        }

        if(blinkTimer <= 0)
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
