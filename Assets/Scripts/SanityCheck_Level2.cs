using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityCheck_Level2 : MonoBehaviour
{
    public Sanity sanity;

    public GameObject sanityLevel2_Appear;
    public GameObject sanityLevel2_Disappear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && sanity.sanityPercentage >= 35f)
        {
            Debug.Log("We can feel something strange in the air");
            sanityLevel2_Appear.SetActive(true);
            sanityLevel2_Disappear.SetActive(false);
        }   
    }
}
