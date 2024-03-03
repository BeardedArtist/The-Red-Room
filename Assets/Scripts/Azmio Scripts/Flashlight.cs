using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEngine.Serialization;

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

    [SerializeField] private Material[] SpinSlotMaterials;
    [SerializeField] private Material[] DiceMaterials;

    [SerializeField,Range(0,10)] private int  totalLoops;
    [SerializeField,Range(0,10)] private float PerLoopDuration;

    [SerializeField,Range(0f,1f)] private float[] DiceOffsets;

    [SerializeField] private GameObject ChoHanChoicePanel;

    private void Start()
    {
        ChoHanChoicePanel.SetActive(false);
    }

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

        SpinSlotOne.sharedMaterial = SpinSlotMaterials[0];
        SpinSlotTwo.sharedMaterial = SpinSlotMaterials[1];
        SpinSlotThree.sharedMaterial = SpinSlotMaterials[2];
        switch (spin)
        {
            //Cho-Han
            case >= 0f and < 1.5f: // Cho - han was picked
                spinResultText.text = "Spin = Cho-Han";

                var DieDigit1 = 1;
                var DieDigit2 = 1;

                mySequence.Append(DOVirtual
                        .Float(0, 1, PerLoopDuration,
                            (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental)
                        .SetEase(Ease.Linear)) // Spins the first slot randomly 10 times
                    .AppendCallback(() =>
                        SpinSlotOne.sharedMaterial.mainTextureOffset =
                            new Vector2(0, 0.54f)) // after spinning 10 times randomly , land on cho-han icon
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration,
                            (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental)
                        .SetEase(Ease.Linear)) //Spins the second slot randomly 10 times
                    .AppendCallback(() =>
                        SpinSlotTwo.sharedMaterial.mainTextureOffset =
                            new Vector2(0, 0.54f)) //after spinning 10 times randomly , land on cho-han
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration,
                            (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                    .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.54f))
                    .Append(DOVirtual.Float(0, 1, 0.1f, (value) =>
                    {
                        SpinSlotOne.sharedMaterial = DiceMaterials[0];
                        SpinSlotTwo.sharedMaterial = DiceMaterials[1];
                        DieDigit1 = Random.Range(1, 7);
                        DieDigit2 = Random.Range(1, 7);
                    }))
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration,
                            (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(SpinSlotOne.sharedMaterial.mainTextureOffset.x, value); })
                        .SetLoops(totalLoops, LoopType.Incremental)
                        .SetEase(Ease.Linear)) 
                    .AppendCallback(() =>
                        SpinSlotOne.sharedMaterial.mainTextureOffset =
                            new Vector2(SpinSlotOne.sharedMaterial.mainTextureOffset.x, DiceOffsets[DieDigit1 - 1]))
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration,
                            (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(SpinSlotTwo.sharedMaterial.mainTextureOffset.x, value); })
                        .SetLoops(totalLoops, LoopType.Incremental)
                        .SetEase(Ease.Linear)) 
                    .AppendCallback(() =>
                        SpinSlotTwo.sharedMaterial.mainTextureOffset =
                            new Vector2(SpinSlotTwo.sharedMaterial.mainTextureOffset.x, DiceOffsets[DieDigit2 - 1]));

                if (DieDigit2 + DieDigit1 / 2 == 0)
                {
                    Debug.Log("Won Cho Han");
                }
                else
                {
                    Debug.Log("Lose Cho Han");
                }
                
                break;
            //Monster
            case >= 1.5f and < 2f:
                spinResultText.text = "Spin = Monster";

                mySequence.Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f))
                .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f))
                .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f));
                break;
            //Light
            case >= 2f and < 7f:
                spinResultText.text = "Spin = Light";

                mySequence.Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.33f))
                .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.33f))
                .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                    .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.33f));
                
                TurnOnFlashlight();
                break;
            
            //Nothing
            case >= 7.5f and <= 10f:
                spinResultText.text = "Spin = Nothing";
                mySequence.Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                    .AppendCallback(() => SpinSlotOne.sharedMaterial.mainTextureOffset = new Vector2(0, 0.33f))
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                    .AppendCallback(() => SpinSlotTwo.sharedMaterial.mainTextureOffset = new Vector2(0, 0.72f))
                    .Append(DOVirtual.Float(0, 1, PerLoopDuration, (value) => { SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, value); })
                        .SetLoops(totalLoops, LoopType.Incremental).SetEase(Ease.Linear))
                    .AppendCallback(() => SpinSlotThree.sharedMaterial.mainTextureOffset = new Vector2(0, 0.54f));
                break;
        }
    }

    private void ContinueChoHanGame()
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
