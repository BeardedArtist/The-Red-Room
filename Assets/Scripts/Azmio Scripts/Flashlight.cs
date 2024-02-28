using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using TMPro;
using MyBox;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private ChoHan _ChoHan;
    [SerializeField] private TMP_Text spinResultText;
    [SerializeField] private SanityControler _sanityControler;
    public Light flashlight;
    public GameObject Camera;
    
    [Range(1.1f,20f)] public float FlashIntensityMultiplier;

    [Range(1f, 100f)] public float FlashingRaycastDistance;
    [MyBox.Tag] public string EnemyTag;

    
    public bool _lightIsOn;
    private float maxIntensity = 8f;
    private float intensityDecreaseRate = 0.05f;

    [SerializeField]private KeyCode SpinKeyInputButton = KeyCode.F;
    [SerializeField]private KeyCode FlashInputButton = KeyCode.Q;

    private RaycastHit RaycastResult;

    [SerializeField] private MeshRenderer SpinSlotOne, SpinSlotTwo, SpinSlotThree;

    [SerializeField,Range(0,10)] private float rotationDuration, totalLoops, speedMultiplier;
    
    private void Update()
    {
        if (Input.GetKeyDown(SpinKeyInputButton))
        {
            Spin();
        }

        Debug.DrawRay(Camera.transform.position,Camera.transform.forward * FlashingRaycastDistance, Color.red);
        if (Input.GetKeyDown(FlashInputButton) && _lightIsOn)
        {
            StartCoroutine(FlashAbility());
        }

        if (_lightIsOn && flashlight.intensity >= 0f)
        {
            flashlight.intensity -= intensityDecreaseRate * Time.deltaTime;
        }
        else
        {
            _lightIsOn = false;
        }
    }

    private void TurnOnFlashlight()
    {
        flashlight.intensity = maxIntensity;
        flashlight.enabled = true;
        _lightIsOn = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/FlashlightOnOff_Updated");
    }
    
    private void Spin()
    {
        var spin = Random.Range(0f, 10f);
        Sequence mySequence = DOTween.Sequence();

        switch (spin)
        {
            //Cho-Han
            case >= 0f and < 1.5f:
                spinResultText.text = "Spin = Cho-Han";

                mySequence.Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.39f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.39f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.39f));


                _ChoHan._rolledChoHan = true;
                break;
            //Monster
            case >= 1.5f and < 2f:
                spinResultText.text = "Spin = Monster";

                mySequence.Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f));


                break;
            //Light
            case >= 2f and < 7f:
                spinResultText.text = "Spin = Light";

                mySequence.Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.1f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.1f))
                .Append(DOVirtual.Float(0, 1, 0.1f, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(10, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.1f));



                TurnOnFlashlight();
                break;
            //Nothing
            case >= 7.5f and <= 10f:
                spinResultText.text = "Spin = Nothing";
                break;
        }
    }

    private void SpinTo(int index)
    {
        
    }

    private IEnumerator FlashAbility()
    {
        var initialIntensity = flashlight.intensity;
        flashlight.intensity *= FlashIntensityMultiplier;

        RaycastEnemy();

        yield return new WaitForSeconds(0.2f);
        flashlight.intensity = initialIntensity;
    
    }

    private void RaycastEnemy()
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastResult, FlashingRaycastDistance))
        {
            
            if (RaycastResult.transform.gameObject.tag == EnemyTag)
            {
                Debug.Log("Hit Enemy Ghost");
                _sanityControler.AlterSanity(40);
            }
            else
            {
                _sanityControler.AlterSanity(-20);
            }
        }
        else
        {
            _sanityControler.AlterSanity(-20);
        }
    }
}
