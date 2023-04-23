using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Animator transition;
    public Animator transition_Disclaimer;
    public float transitionTime = 1f;

    public void StartGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI Sounds/Menu UI DETUNE", GetComponent<Transform>().position);
        loadGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI Sounds/Menu UI DETUNE", GetComponent<Transform>().position);
        Application.Quit();
    }

    public void loadGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        transition_Disclaimer.SetTrigger("StartDisclaimer");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
