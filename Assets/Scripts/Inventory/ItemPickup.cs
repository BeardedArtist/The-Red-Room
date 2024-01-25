using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    private bool trig;
    [SerializeField] private GameObject pickupUI;
    [SerializeField] private GameObject[] ObjectsToAppear;

    void Pickup()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Item Interaction/KEYSPICKUPECHO", GetComponent<Transform>().position);
        Destroy(gameObject);
        InventoryManager.Instance.Add(Item);
        pickupUI.SetActive(false);

        for (int j = 0; j < ObjectsToAppear.Length; j++)
        {
            if (ObjectsToAppear[j] == null)
            {
                return;
            }
            else
            {
                ObjectsToAppear[j].SetActive(true);
            }
        }
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
