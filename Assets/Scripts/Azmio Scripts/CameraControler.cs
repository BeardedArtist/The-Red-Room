using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Animator camera_Anim;
    [SerializeField] private ObjectManager objectManager;


    void Start()
    {
        camera_Anim.Play("TakeOut_Camera", 0, 0.1f);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            camera_Anim.SetTrigger("CameraAway");
            objectManager._CameraEnabled = false;
        }

        //camera_Anim.ResetTrigger("CameraOut");
    }
}
