using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityControler : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private float maxSanity; // Make it so that this also displays the Sanity value going down
    [SerializeField] private float sanityDecreaseRate;  // Amount of sanity decrease per seconds
    private float currentSanity;
    private float everySecondTimer = 1f;


    void Start()
    {
        currentSanity = maxSanity;
        UpdateSanityUI();
    }


    void Update()
    {
        everySecondTimer -= Time.deltaTime;

        if (everySecondTimer <= 0 && currentSanity > 0)
        {
            _DecreaseSanity(sanityDecreaseRate);

            if (currentSanity <= 50)
            {
                _playerMovement.sprintSpeed -= 2f / 50; // Sprint speed - walk speed = currently equals to 2, so we divide it by the 50 seconds left.
            }

            everySecondTimer = 1f;
        }

        // Test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentSanity = 50;
        }
    }


    public void _DecreaseSanity(float decreaseAmount)
    {
        currentSanity -= decreaseAmount;
        UpdateSanityUI();
    }


    void UpdateSanityUI() // Update the slider value
    {
        sanitySlider.value = currentSanity / maxSanity;
    }
}
