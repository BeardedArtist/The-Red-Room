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

    public PlayerMovement playerMovement;

    [Range(1f, 100f),SerializeField] private float DistanceFromPlayerToSlowDown; //@Azmio , Set Accordingly
    player Player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GetComponent<player>();

        agent.speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerTransform.position;
        playerMovement.IsMonsterNearby = Vector3.Distance(transform.position, playerTransform.position) < DistanceFromPlayerToSlowDown;
    }
}
