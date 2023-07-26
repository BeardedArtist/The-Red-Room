using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lock : MonoBehaviour
{
    public TMP_InputField inputField;

    const string correctCombination = "vile";

    void Update()
    {
        //if enter pressed
        //CheckCombination();
    }

    public void CheckCombination()
    {
        string input = inputField.text.ToLower(); // Convert the input to lowercase for case insensitivity

        if (input == correctCombination)
        {
            //Letters turn green
            gameObject.SetActive(false); // Disable the door GameObject to open it | Launch Animation
        }
        else
        {
            //Letters turn red AND dissapear;
        }
    }
}
