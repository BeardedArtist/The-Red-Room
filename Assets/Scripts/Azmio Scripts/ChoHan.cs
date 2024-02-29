using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoHan : MonoBehaviour
{
    // [SerializeField] SanityControler _sanityControler;
    // [SerializeField] TMP_Text betChoiceText;
    // [SerializeField] TMP_Text resultText;
    // [SerializeField] TMP_Text winLossText;
    //
    // public bool _rolledChoHan = false;
    // bool canClick = true;
    // bool isDoubles = false;
    //
    //
    // void Update() // Obsolete Script , please discard
    // {
    //     if (_rolledChoHan)
    //     {
    //         betChoiceText.gameObject.SetActive(true);
    //
    //         if (Input.GetKeyDown(KeyCode.Alpha1))
    //         PlayGame(true);
    //
    //         else if (Input.GetKeyDown(KeyCode.Alpha2))
    //         PlayGame(false);
    //     }
    // }
    //
    //
    // void PlayGame(bool isEven)
    // {
    //     int diceRoll1 = Random.Range(1, 7);
    //     int diceRoll2 = Random.Range(1, 7);
    //
    //     //Calculations
    //     int total = diceRoll1 + diceRoll2;
    //     bool result = total % 2 == 0;
    //     resultText.text = total.ToString();
    //
    //     if (diceRoll1 == diceRoll2)
    //     {
    //         isDoubles = true;
    //         resultText.text = total.ToString() + " (Doubles)";
    //     }
    //
    //     if ((result && isEven) || (!result && !isEven)) // Player wins
    //     {
    //         winLossText.text = "You Picked Even = " + isEven + " And you Won!";
    //         
    //         if (isDoubles)
    //         {
    //             _sanityControler.currentSanity += total * 2;
    //         }
    //         else
    //         {
    //             _sanityControler.currentSanity += total;
    //         }
    //         
    //     }
    //     else // Player loses
    //     {
    //         winLossText.text = "You Picked Even = " + isEven + " And you Lost!";
    //     }
    //     
    //     betChoiceText.gameObject.SetActive(false);
    //     isDoubles = false;
    //     _rolledChoHan = false;
    // }
}
