using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Interacttext;


    [Range(1f,10f)] public float InteractionRange;
    private RaycastHit HitInfo;


    private void Update()
    {
        if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo,InteractionRange)){
            if(HitInfo.transform.GetComponent<Interactable>()!=null){
                Interacttext.SetActive(true);
                Debug.Log(HitInfo.transform.gameObject.name);
                var interactable = HitInfo.transform.GetComponent<Interactable>();
                if(Input.GetKeyDown(KeyCode.E)){
                     Interacttext.SetActive(false);
                    interactable.Interact();
                    interactable.GetComponent<Collider>().enabled = false;
                }
            }
            else{
                Interacttext.SetActive(false);
            }
        }
        else{
             Interacttext.SetActive(false);
        }
    }
}
