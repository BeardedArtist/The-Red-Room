using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_StalkerController : MonoBehaviour
{
    [Header("AI Parameters")]
    [SerializeField] private string state = "idle";
    NavMeshAgent agent;

    [Header("Player Parameters")]
    public Transform playerTransform;
    public GameObject mainPlayer;
    public MeshRenderer mainPlayerMesh;
    player Player;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GetComponent<player>();

        agent.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerTransform.position;
    }
}
