using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrap : MonoBehaviour
{
    // Script to shut the door behind the player

    // TODO:
    // --> Have door animate shut behind them.
    // --> Have shutting door audio (FMOD)
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private Animator myBookshelf = null;
    [SerializeField] private Animator myLamp = null;
    private bool trigger;
    private bool bookshelf_trigger;
    private bool lamp_trigger;
    private bool trig;
    private bool hasDoorClosed = false;

    [SerializeField] private OpenCloseDoor openCloseDoor;
    [SerializeField] private OpenCloseDoor_LOCKED openCloseDoor_LOCKED;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            trig = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        trig = false;    
    }

    private void Update() 
    {
        if (trig == true)
        {
            trigger = myDoor.GetBool("Open");

            if (trigger && hasDoorClosed == false)
            {
                StartCoroutine(DelayAnimation());
                myDoor.SetBool("Open", false); // close door via bool
                hasDoorClosed = true;

                openCloseDoor.enabled = false;
                openCloseDoor_LOCKED.enabled = true;

                // ADD AUDIO
            }

            if (!trigger)
            {
                openCloseDoor.enabled = false;
                openCloseDoor_LOCKED.enabled = true;
            }
        }
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(2.0f);
        bookshelf_trigger = myBookshelf.GetBool("isFalling");
        myBookshelf.SetBool("isFalling", true);

        yield return new WaitForSeconds(2.5f);
        lamp_trigger = myLamp.GetBool("isLampFalling");
        myLamp.SetBool("isLampFalling", true);
    }
}
