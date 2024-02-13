using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
   [Foldout("Component References", true)] 
   [SerializeField] private GameObject PauseMenu;
   [SerializeField] private KeyCode PauseButton;
   [SerializeField] private Slider BrightnessSlider, VolumeSlider, SensitivitySlider;

   [Foldout("Player Values", true)] 
   [SerializeField] private MouseLook _mouseLook;
   
   #region Delegates
   public delegate void OnEventOccured(bool status);
   public static OnEventOccured MenuStatusToggled;
   #endregion
    
   private void Start()
   {
      PauseMenu.SetActive(false);
      SensitivitySlider.value = _mouseLook.mouseSensitivity;
   }

   //@Azmio Using Update here because we're sticking to the old Input System , the new one uses callbacks and listeners and is much more efficient
   //Maybe in future we'll Move it to new Input System
   private void Update()
   {
      if (Input.GetKeyDown(PauseButton))
      {
         ToggleMenu();
      }
   }

   public void ToggleMenu()
   {
      Cursor.lockState = PauseMenu.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
      PauseMenu.SetActive(!PauseMenu.activeSelf);
      MenuStatusToggled(PauseMenu.activeSelf);
   }

   public void UpdateSensitivity()
   {
      _mouseLook.UpdatedMouseSensitivity = SensitivitySlider.value;
   }
   
}
