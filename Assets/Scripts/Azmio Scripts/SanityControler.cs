using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityControler : MonoBehaviour
{
    public Slider sanitySlider;
    public float maxSanity = 100f;
    public float sanityDecreaseRate = 0.5f;  // Amount of sanity decrease per seconds
    float currentSanity;


    void Start()
    {
        currentSanity = maxSanity;
        UpdateSanityUI();
        InvokeRepeating("DecreaseSanity", 1f, 1f);  // Start decreasing sanity every second
    }


    void DecreaseSanity()
    {
        if (currentSanity > 0f)
        {
            currentSanity -= sanityDecreaseRate;
            UpdateSanityUI();
        }
    }


    void UpdateSanityUI()
    {
        sanitySlider.value = currentSanity / maxSanity;  // Update the slider value
    }
}
