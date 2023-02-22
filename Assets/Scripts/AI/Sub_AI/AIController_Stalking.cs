using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController_Stalking : MonoBehaviour
{
    public Transform[] pointsToWalkTo;
    public NavMeshAgent agent;
    private bool isWalking = false;

    [SerializeField] private GameObject flashlight_Light;
    [SerializeField] private Flashlight flashlight_Script;

    Animator anim;

    private void Start() 
    {
        agent = GetComponent<NavMeshAgent>();    
        anim = GetComponent<Animator>();
    }


    public void ActivateAI()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        for (int i = 0; i < pointsToWalkTo.Length; i++)
        {
            if (pointsToWalkTo[i] == null)
            {
                flashlight_Light.SetActive(false);
                flashlight_Script.lightIsOn = false;
                gameObject.SetActive(false);
                Debug.Log("Destroy");
            }

            else
            {
                anim.SetBool("isWalking", true);
                isWalking = true;
                agent.destination = pointsToWalkTo[i].position;

                if (agent.remainingDistance < 0.1)
                {
                    Destroy(gameObject);
                    isWalking = false;
                }
            }
        }
    }
}
