using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private GameObject cameraGO;
    [SerializeField] private Animator camera_Anim;


    void Start()
    {
        cameraGO.SetActive(false);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)); //CameraGO Enabled by default when game starts because of script being called from Main Camera
        {
            cameraGO.SetActive(true);
        }
    }
}
