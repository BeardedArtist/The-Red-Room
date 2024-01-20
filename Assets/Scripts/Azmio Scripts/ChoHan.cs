using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoHan : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text winLossText;
    [SerializeField] Blink blink_Script;
    
    float clickCooldown = 0.05f;
    bool canClick = true;
    bool isDoubles = false;
    

    void Update()
    {
        if (canClick && Input.GetKey(KeyCode.F))
        {
            blink_Script.enabled = false;

            if (Input.GetMouseButtonDown(0))
            PlayGame(true);

            else if (Input.GetMouseButtonDown(1))
            PlayGame(false);
        }
    }


    void PlayGame(bool isEven)
    {
        int diceRoll1 = Random.Range(1, 7);
        int diceRoll2 = Random.Range(1, 7);

        //Calculations
        int total = diceRoll1 + diceRoll2;
        bool result = total % 2 == 0;
        resultText.text = total.ToString();

        if (diceRoll1 == diceRoll2)
        {
            isDoubles = true;
            resultText.text = total.ToString() + " Doubles";
        }

        if ((result && isEven) || (!result && !isEven))
        {
            // Player wins
            winLossText.text = "You Picked Even = " + isEven + " And you Won!";
        }

        else
        {
            // Player loses
            winLossText.text = "You Picked Even = " + isEven + " And you Lost!";
        }
        
        isDoubles = false;
        StartCoroutine(ClickCooldown());
    }


    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(clickCooldown);
        canClick = true;
    }
}
