using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Animator camera_Anim;


    void Start()
    {
        gameObject.SetActive(false);
    }


    void Update()
    {

    }
}
