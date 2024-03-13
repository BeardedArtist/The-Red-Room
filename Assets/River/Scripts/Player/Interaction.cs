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
                var interactable = HitInfo.transform.GetComponent<Interactable>();

                if (interactable.DontTriggerByRaycast) return;

                Interacttext.SetActive(true);

                if (interactable.type == Interactable.InteractableType.ObjectRemoval)
                {
                    HoldEtext.SetActive(true);
                }

                if (!Input.GetKeyDown(KeyCode.E)) return;
                Interacttext.SetActive(false);
                interactable.Interact();
                interactable.GetComponent<Collider>().enabled = false;
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
