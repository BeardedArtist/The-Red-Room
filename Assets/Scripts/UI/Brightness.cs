using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Brightness : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;
    public Slider brightnessSlider;

    private void Start() 
    {
        //volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);

        Test(brightnessSlider.value);

    }

    public void Test(float value)
    {
        if (value != 0)
        {
            colorAdjustments.postExposure.value = value;
        }
        else
        {
            colorAdjustments.postExposure.value = 0f;
        }
    }











    // --- ORIGINAL CODE ----


    // public Slider brightnessSlider;

    // public PostProcessProfile brightness;
    // public PostProcessLayer layer;

    // AutoExposure exposure;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     brightness.TryGetSettings(out exposure);
    //     AdjustBrightness(brightnessSlider.value);
    // }

    // public void AdjustBrightness(float value)
    // {
    //     if (value != 0)
    //     {
    //         exposure.keyValue.value = value;
    //     }
    //     else
    //     {
    //         exposure.keyValue.value = .05f;
    //     }
    // }
}
