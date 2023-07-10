using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{
    public InputField inputField;

    const string correctCombination = "vile";

    void Update()
    {
        //if enter pressed && inputfiled active then
        //CheckCombination();
    }

    public void CheckCombination()
    {
        string input = inputField.text.ToLower(); // Convert the input to lowercase for case insensitivity

        if (input.Length != 4)
        {
            Debug.Log("Invalid combination! Combination must have exactly 4 letters.");
            return;
        }

        if (input == correctCombination)
        {
            Debug.Log("Correct combination! The door opens.");
            gameObject.SetActive(false); // Disable the door GameObject to open it
        }
        else
        {
            Debug.Log("Incorrect combination! The door remains locked.");
        }
    }
}
