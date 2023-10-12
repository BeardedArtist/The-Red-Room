using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Animator camera_Anim;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private float cameraTimer;
    private float cameraMaxTime;


    void Start()
    {
        cameraMaxTime = cameraTimer;
        
        camera_Anim.Play("TakeOut_Camera", 0, 0);
    }


    void Update()
    {
        cameraTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.C) || cameraTimer <= 0)
        {
            StartCoroutine(AnimationOutTimer());
        }
    }

    IEnumerator AnimationOutTimer()
    {
        camera_Anim.SetTrigger("CameraAway");

        yield return new WaitForSeconds(1f);

        cameraTimer = cameraMaxTime;
        camera_Anim.ResetTrigger("CameraOut");
        objectManager._CameraEnabled = false;
    }
}
