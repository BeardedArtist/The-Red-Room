using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoHan : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text winLossText;
    [SerializeField] private Blink blink_Script;
    
    private float clickCooldown = 0.05f;
    private bool canClick = true;
    

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

        int total = diceRoll1 + diceRoll2;
        bool result = total % 2 == 0;

        resultText.text = total.ToString();

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

        StartCoroutine(ClickCooldown());
    }


    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(clickCooldown);
        canClick = true;
    }
}
