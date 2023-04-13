using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act1_Final_Demo : MonoBehaviour
{
    // Object References
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject AI_FINAL;
    [SerializeField] private GameObject Chase_AI_DISABLE;
    [SerializeField] private GameObject Hunting_AI_Backup_DISABLE;

    // Transform Reference
    [SerializeField] private Transform warpTarget;

    // Bool Reference
    private bool trig;

    // Script References
    [SerializeField] private MouseLook mouseLook_Script;
    [SerializeField] private PlayerMovement playerMovement_Script;
    [SerializeField] private OpenCloseDoor openCloseDoor_Script;
    [SerializeField] private PauseMenu pauseMenu_Script;
    private CharacterController characterController_Script;
    // Turn off blink script


    // Animator References
    [SerializeField] private Animator blink_Anim;
    [SerializeField] private Animator blink_Anim_2;
    private bool hasAnimationPlayed = false;

    // Door Collider
    [SerializeField] private Collider doorCollider;


    // Start is called before the first frame update
    void Start()
    {
        characterController_Script = player.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && hasAnimationPlayed == false)
        {
            blink_Anim.Play("TopLidBlink", 0, 0.25f);
            blink_Anim_2.Play("BottomLidBlink", 0, 0.25f);
            Chase_AI_DISABLE.SetActive(false);
            Hunting_AI_Backup_DISABLE.SetActive(false);
            trig = true;
            hasAnimationPlayed = true;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (trig)
        {
            StartCoroutine(ActivateFinalScene());
        }
    }

    IEnumerator ActivateFinalScene()
    {
        yield return new WaitForSeconds(0.40f);

        AI_FINAL.SetActive(true);

        characterController_Script.enabled = false;
        player.transform.position = warpTarget.transform.position;
        player.transform.rotation = warpTarget.transform.rotation;
        characterController_Script.enabled = true;

        doorCollider.enabled = false;
        mouseLook_Script.enabled = false;
        playerMovement_Script.enabled = false;
        openCloseDoor_Script.enabled = false;
        pauseMenu_Script.enabled = false;
    }
}
