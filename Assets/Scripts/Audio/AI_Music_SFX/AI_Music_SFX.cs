using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AI_Music_SFX : MonoBehaviour
{
    //[SerializeField] EventReference eventName;
    //private static FMOD.Studio.EventInstance Music_SFX;
    bool hasAudioPlayed = false;
    public float timer = 30f;

    // Update is called once per frame
    void Update()
    {
        if (hasAudioPlayed == false)
        {
            hasAudioPlayed = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Monster/Choking SFX", GetComponent<Transform>().position);
            timer -= 1 * Time.deltaTime;
        }

        if (timer <= 0)
        {
            hasAudioPlayed = false;
        }

    }
}
