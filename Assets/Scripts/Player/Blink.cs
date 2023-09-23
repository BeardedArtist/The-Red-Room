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
    [SerializeField] private float timer;
    float blinkTimer = 2f;

    void Start() 
    {
        timer = Random.Range(40.0f, 60.0f); 
    }


    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !isBlinking)
        {
            timer = Random.Range(40.0f, 60.0f);
            isBlinking = true;
        }

        if (Input.GetMouseButton(1) && !isBlinking)
        {
            isBlinking = true;
        }
        
        if (isBlinking == true)
        {
            BlinkActive();
            blinkTimer -= Time.deltaTime;
            // isBlinking is true and therefor activates the animation for the entire 2sec it's true

            if(blinkTimer != 0)
            {
                isBlinking = false;
                blinkTimer = 2f;
            }
        }
    }


    void BlinkActive()
    {
        blink_Anim.Play("TopLidBlink", 0, 0.25f);
        blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
        Debug.Log("yeah");
    }
}
