using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSceneTransition : MonoBehaviour
{
    [SerializeField] private StartMenu startMenu_Script;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelaySceneTransition());
    }

    IEnumerator DelaySceneTransition()
    {
        yield return new WaitForSeconds(52f);

        startMenu_Script.loadGame();
    }
}
