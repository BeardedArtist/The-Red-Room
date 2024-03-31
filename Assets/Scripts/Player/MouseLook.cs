using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(0f,100f)]public float mouseSensitivity = 100f; // apply a speed to mouse movement.

    public float xRotation = 0f;
    private float mouseX, mouseY;
    public Transform playerBody;
    
    public bool CanLook = true;

    [HideInInspector] public float UpdatedMouseSensitivity;

    public static MouseLook instance; // Singleton
    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null) instance = this;
        UpdatedMouseSensitivity = mouseSensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenuManager.MenuStatusToggled += (opened) =>
        {
            if (opened)
            {
                UpdatedMouseSensitivity = mouseSensitivity;
                mouseSensitivity = 0f; 
            }
            else
            {
                mouseSensitivity = UpdatedMouseSensitivity; 
            }
        };
    }


    private void Update()
    {
        if (CanLook)
        {
            // TEST - Trying without Time.deltaTime;
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // get preprogrammed input for mouse on x-axis.
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // get preprogrammed input for mouse on x-axis.
            // TEST - Trying without Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limit player camera movement for up and down. 


            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX); // allows for players to look left/right w/mouse.
        }
        else
        {
            // Convert the angle to -180 to 180 range
            float angle = transform.rotation.eulerAngles.x;
            if (angle > 180)
                angle -= 360;
            xRotation = angle;
            //Debug.Log(angle);
        }
    }
}
