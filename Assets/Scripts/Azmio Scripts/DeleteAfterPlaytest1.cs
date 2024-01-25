using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterPlaytest1 : MonoBehaviour
{
    [SerializeField] GameObject ladderToVent;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ladderToVent.SetActive(true);
        }
    }
}
