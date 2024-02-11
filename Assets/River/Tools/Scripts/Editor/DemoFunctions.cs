using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoFunctions : MonoBehaviour
{

    public static void DemoFunction(int i){
        switch (i)
        {
            case 0:
                Debug.Log("Pressed Button Index :" + i.ToString());
                break;
            case 1:
                Debug.Log("Pressed Button Index :" + i.ToString());
                break;
        }
        
    }
}
