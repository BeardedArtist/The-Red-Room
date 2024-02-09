using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject Camera;

    [Range(1f,10f)] public float InteractionRange;
    public RaycastHit HitInfo;


    void Update()
    {
        if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out HitInfo,InteractionRange)){
            if(HitInfo.transform.GetComponent<Interactable>()!=null){
                Debug.Log(HitInfo.trasform.gameObject.name);
                var interactable = HitInfo.transform.GetComponent<Interactable>();
                if(Input.GetMouseButtonDown(0)){
                    interactable.Interact();
                }
            }
        }
    }
}
