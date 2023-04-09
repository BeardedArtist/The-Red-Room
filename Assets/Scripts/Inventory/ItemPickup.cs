using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    private bool trig;

    [SerializeField] private GameObject pickupUI;

    void Pickup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/KEYSPICKUPECHO", GetComponent<Transform>().position);
        Destroy(gameObject);
        InventoryManager.Instance.Add(Item);
        pickupUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Flashlight Eyes 2")
        {
            trig = true;
            pickupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        trig = false;
        pickupUI.SetActive(false);
    }

    private void Update() 
    {
        if (trig)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }    
        }
    }
}
