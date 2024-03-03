using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Interacttext;
    public GameObject HoldEtext;


    [Range(1f, 10f)] public float InteractionRange;
    private RaycastHit HitInfo;


    private void FixedUpdate()
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo, InteractionRange))
        {
            if (HitInfo.transform.GetComponent<Interactable>() != null)
            {
                Interacttext.SetActive(true);
                //Debug.Log(HitInfo.transform.gameObject.name);
                var interactable = HitInfo.transform.GetComponent<Interactable>();
                // if(interactable.type == Interactable.InteractableType.FishBowl){
                    
                // }
                if (!Input.GetKeyDown(KeyCode.E)) return;
                Interacttext.SetActive(false);
                interactable.Interact();
                interactable.GetComponent<Collider>().enabled = false;
            }

            else if (HitInfo.transform.GetComponent<PlaytestOnlyObjectRemove>() != null)
            {
                HoldEtext.SetActive(true);
                HitInfo.transform.GetComponent<PlaytestOnlyObjectRemove>().PlankDeactivate();
            }

            else
            {
                Interacttext.SetActive(false);
                HoldEtext.SetActive(false);
            }
        }
        else
        {
            Interacttext.SetActive(false);
            HoldEtext.SetActive(false);
        }
    }
}
