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

    public float currentSanity;
    public float sprintSpeedDecreaseRate;
    private float everySecondTimer = 1f;


    private void Start()
    {
        currentSanity = maxSanity;
        UpdateSanityUI();
    }


    private void Update()
    {
        everySecondTimer -= Time.deltaTime;

        if (everySecondTimer <= 0 && currentSanity > 0)
        {
            AlterSanity(-sanityDecreaseRate);

            if (currentSanity <= 50)
            {
                _playerMovement.sprintSpeed -= sprintSpeedDecreaseRate / 50; // Sprint speed - walk speed = currently equals to 2, so we divide it by the 50 seconds left to get a smooth slow down.
            }

            everySecondTimer = 1f;
        }
    }


    public void AlterSanity(float decreaseAmount)
    {
        currentSanity += decreaseAmount;
        UpdateSanityUI();
    }


    private void UpdateSanityUI() // Update the slider value
    {
        sanitySlider.value = currentSanity / maxSanity;
    }
}
