using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSubtitlesOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Recording_X_Subtitles;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Recording_X_Subtitles.SetActive(true);
        }    
    }
}
