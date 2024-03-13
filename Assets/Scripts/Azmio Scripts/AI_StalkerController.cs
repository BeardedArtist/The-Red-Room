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

    public static AI_StalkerController instance;
    [Range(0f, 30f), SerializeField] private float CrouchHeight, StandHeight;
    [SerializeField] private GameObject MonsterCamera;
    
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GetComponent<player>();
        instance = this;
        agent.speed = 1.7f;
    }


    private void Update()
    {
        var targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

        agent.destination = targetPosition;
        playerMovement.IsMonsterNearby = Vector3.Distance(transform.position, playerTransform.position) < DistanceFromPlayerToSlowDown;
        //Debug.Log("Distance From Player"+Vector3.Distance(transform.position, playerTransform.position));
    }

    public void Crouch()
    {
        var MonsterCameraPosition = MonsterCamera.transform.position;
        MonsterCameraPosition.y = CrouchHeight;
        MonsterCamera.transform.position = MonsterCameraPosition;
    }

    public void UnCrouch()
    {
        var MonsterCameraPosition = MonsterCamera.transform.position;
        MonsterCameraPosition.y = StandHeight;
        MonsterCamera.transform.position = MonsterCameraPosition;
    }
}
