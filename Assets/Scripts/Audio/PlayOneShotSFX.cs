using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayOneShotSFX : MonoBehaviour
{
    // This script should play Audio only once! 

    private bool hasAudioPlayed = false;
    [SerializeField] private string eventName;
    [SerializeField] private float waitTime;


    public void PlaySfxOnce()
    {
        if (hasAudioPlayed == false)
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventName);
            hasAudioPlayed = true;
        }
    }

    public void PlaySfxOnce_3D()
    {
        if (hasAudioPlayed == false)
        {
            StartCoroutine(PlaySfxWithTime());
            hasAudioPlayed = true;
        }
    }


    IEnumerator PlaySfxWithTime()
    {
        yield return new WaitForSeconds(waitTime);
        FMODUnity.RuntimeManager.PlayOneShot(eventName, GetComponent<Transform>().position);
    }
}
