using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestOnlyObjectRemove : MonoBehaviour
{
    private float holdTimer = 0;
    private float holdLenght = 2f;


    public void PlankDeactivate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdLenght)
            {
                gameObject.SetActive(false);
                holdTimer = 0;
            }
        }
        
    }
}
