using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlesForRecordings : MonoBehaviour
{
    [SerializeField] private GameObject Recording;
    [SerializeField] private float WaitTimer;

    [SerializeField] private GameObject[] Subtitles_UI;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateSubtitles());
    }

    IEnumerator ActivateSubtitles()
    {
        for (int i = 0; i < Subtitles_UI.Length; i++)
        {
            Subtitles_UI[i].SetActive(true);
            yield return new WaitForSeconds(WaitTimer);
            Subtitles_UI[i].SetActive(false);
        }
        Recording.SetActive(false);
    }
}
