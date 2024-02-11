using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Config : MonoBehaviour
{
    public struct ButtonConfig{
        public int index;
        public string description;
      //  public UnityAction action;
        //Add action Code
    }

    public static List<ButtonConfig> _ButtonConfig = new List<ButtonConfig>();

    public static void Initialize(){
        ButtonConfig config = new ButtonConfig();

        for(int i =1;i<=CustomTools.progress;i++){
            switch(i){
                case 8:
                config.index = 8;
                config.description = "Eight";
                break;

                case 7:
                config.index = 7;
                config.description = "Seven";
                break;

                case 6:
                config.index = 6;
                config.description = "Six";
                break;

                case 5:
                config.index = 5;
                config.description = "Five";
                break;

                case 4:
                config.index = 4;
                config.description = "Four";
                break;

                case 3:
                config.index = 3;
                config.description = "Thwee";
                break;

                case 2:
                config.index = 2;
                config.description = "Light Sky";
                break;

                case 1:
                config.index = 1;
                config.description = "Dark Sky";
               // config.action += DemoFunctions.DemoFunction;
                break;
               
                default:
                config.index = 9;
                config.description = "not defined";
                break;
            }

            _ButtonConfig.Add(config);
        }

    }

    public static void Clear(){
        
        _ButtonConfig.Clear();


    }
   
}
