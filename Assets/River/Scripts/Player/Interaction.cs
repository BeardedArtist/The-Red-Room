using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Interacttext;
    public GameObject HoldEtext;

    [Range(1f, 10f)] public float InteractionRange;
    private RaycastHit HitInfo;

    [SerializeField, Range(0f, 10000f)] private float ThrowForce = 1000f;
    private bool HoldingPaper;
    [SerializeField] private Transform PaperHolder;
    [SerializeField, ReadOnly] private GameObject Paper;

    [SerializeField] GameObject betChoiceText;

    private void FixedUpdate()
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo, InteractionRange))
        {
            if (HitInfo.transform.GetComponent<Interactable>() != null)
            {
                var interactable = HitInfo.transform.GetComponent<Interactable>();

                if (interactable.DontTriggerByRaycast) return;

                Interacttext.SetActive(true);

                if (interactable.type == Interactable.InteractableType.ChoHan && Input.GetKeyDown(KeyCode.E))
                {
                    betChoiceText.SetActive(true);
                    return;
                }

                if (interactable.type == Interactable.InteractableType.ObjectRemoval)
                {
                    HoldEtext.SetActive(true);
                }

                if (!Input.GetKeyDown(KeyCode.E)) return;
                
                Interacttext.SetActive(false);
                interactable.Interact(false);
                //interactable.GetComponent<Collider>().enabled = false;

                if (interactable.type == Interactable.InteractableType.Paper)
                {
                    HoldingPaper = true;
                    interactable.gameObject.transform.SetParent(PaperHolder.transform);
                    interactable.gameObject.transform.position = PaperHolder.transform.position;
                    Paper = interactable.gameObject;
                    Paper.GetComponent<Rigidbody>().useGravity = false;
                    Paper.GetComponent<Rigidbody>().isKinematic = true;

                }
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

        if (HoldingPaper && Input.GetMouseButtonDown(0))
        {
            Paper.gameObject.transform.SetParent(null);
            Paper.GetComponent<Collider>().enabled = true;
            Paper.GetComponent<Rigidbody>().isKinematic = false;
            Paper.GetComponent<Rigidbody>().useGravity = true;
            Paper.GetComponent<Rigidbody>().AddForce(PaperHolder.transform.forward * ThrowForce, ForceMode.Force);
            Paper = null;
            HoldingPaper = false;
        }
    }


}
