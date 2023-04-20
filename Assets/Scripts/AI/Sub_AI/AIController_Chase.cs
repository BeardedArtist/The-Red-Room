using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController_Chase : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject playerCamera;
    NavMeshAgent agent;

    [SerializeField] private string state = "chase";
    private float wait = 0f;
    private bool highAlert = false;
    private float alertTime = 50f;


    public GameObject deathCam;
    public Transform deathCamPosition;
    public GameObject mainPlayer;
    public MeshRenderer mainPlayerMesh;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Blink blink_script;

    Animator animator;


    // Video for death scene
    [SerializeField] GameObject DeathVideo;
    // Video for death scene

    // AI to be added upon player death
    [SerializeField] GameObject backupAI;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        blink_script.enabled = false;

        StartCoroutine(DelayForChase());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "chase")
        {
            agent.destination = playerTransform.position;

            if (agent.remainingDistance <= agent.stoppingDistance + 1f && !agent.pathPending)
            {
                state = "kill";
                mainPlayer.GetComponent<PlayerMovement>().enabled = false;
                mainPlayerMesh.enabled = false;
                deathCam.SetActive(true);
                deathCam.transform.position = Camera.main.transform.position;
                deathCam.transform.rotation = Camera.main.transform.rotation;
                Camera.main.gameObject.SetActive(false);
                Invoke("reset", 1f);
            }
        }

        if (state == "kill")
        {
            deathCam.transform.position = Vector3.Slerp(deathCam.transform.position, deathCamPosition.position, 10f * Time.deltaTime);
            deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation, deathCamPosition.rotation, 10f * Time.deltaTime);
            agent.SetDestination(deathCam.transform.position);
            agent.speed = 0f;
        }

        HandleAnimation();


        if (gameObject == null)
        {
            blink_script.enabled = true;
        }
    }

    void reset() 
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        StartCoroutine(ActivateDeathScene());
    }


    void HandleAnimation()
    {
        if (state == "chase")
        {
            animator.SetBool("isChasing", true);
        }

        if (state == "kill")
        {
            animator.SetBool("isChasing", false);
        }
    }


    IEnumerator ActivateDeathScene()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Calling This");

        playerCamera.SetActive(true);
        DeathVideo.SetActive(true);

        playerMovement.HandleDeath();
        mainPlayer.GetComponent<PlayerMovement>().enabled = true;
        deathCam.SetActive(false);

        Camera.main.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);
        DeathVideo.SetActive(false);
        agent.speed = 0;

        // Activate new AI
        gameObject.SetActive(false);
        backupAI.SetActive(true);

    }

    IEnumerator DelayForChase()
    {
        state = "idle";
        agent.speed = 0;
        yield return new WaitForSeconds(2f);
        state = "chase";
        agent.speed = 3.8f;
        
    }
}
