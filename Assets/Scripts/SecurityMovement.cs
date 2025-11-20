using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SecurityMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTarget;
    [SerializeField] Transform[] patrolTargets;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private GameObject alarmObject;
    [SerializeField] private GameObject electricityBox;
    private ElectricityBox electricityBoxScript;
    private Alarm alarmScript;
    [SerializeField] private string Status;
    private List<string> Statuses;
    private string nextStatus;
    private Vector3 alarmPos;
    [SerializeField] private float waitTime = 5f;

    [SerializeField] private GameObject SCFovPos;
    [SerializeField] private GameObject SCFov;

    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;
    private FieldOfView enemyFieldOfView;

    private LayerMask layerMask;


    NavMeshAgent agent;
    void Start()
    {
        layerMask = LayerMask.GetMask("Object", "Player");
        enemyFieldOfView = SCFov.GetComponent<FieldOfView>();
        enemyFieldOfView.SetValues(viewDistance, viewAngle);
        electricityBoxScript = electricityBox.GetComponent<ElectricityBox>();
        alarmScript = alarmObject.GetComponent<Alarm>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Statuses = new List<string>() {"Idle", "Patrol", "Alarm", "Box", "Chase"};
        nextStatus = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (alarmScript.isChase)
        {
            nextStatus = "Chase";
        }
        else if (!alarmScript.isChase && Status == "Chase")
        {
            nextStatus = "Idle";
        }
        enemyFieldOfView.SetOrigin(transform.position);
        enemyFieldOfView.SetAimDirection((SCFovPos.transform.position - transform.position).normalized);

        if (Statuses.IndexOf(nextStatus) > Statuses.IndexOf(Status) || nextStatus == "Idle" || nextStatus == "Chase") // Status Priority
        {
            if (nextStatus == "Idle")
            {
                Status = "Patrol";
                nextStatus = "";
            }
            else
            {
                Status = nextStatus;
                nextStatus = "";
            }
           targetPos = DetermineTarget();
        }

        MoveToTarget(targetPos);
        WaitAtTarget();
        FaceTarget();
        FindTargetPlayer();
    }
    void MoveToTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }
    void WaitAtTarget()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        if (Vector3.Distance(transform.position, electricityBox.transform.position) <= 2f)
                        {
                            if (!electricityBoxScript.GetIsElectricBoxActive())
                            {
                                electricityBoxScript.TurnOnOff();
                            }
                        }
                        EndHelp();
                    }
                }
            }
        }
    }
    Vector3 DetermineTarget()
    {
        Vector3 posit = transform.position;
        switch (Status)
        {
            case "Patrol":
                agent.speed = 1f;
                waitTime = 5f;
                posit = patrolTargets[Random.Range(0, patrolTargets.Length)].position;
                break;
            case "Alarm":
                waitTime = 3f;
                agent.speed = 1.3f;
                posit = alarmPos;
                break;
            case "Box":
                waitTime = 10f;
                posit = electricityBox.transform.position;
                break;
            case "Chase":
                agent.speed = 1.5f;
                posit = playerTarget.position;
                break;
        }
        return posit;
    }
    public void CallForElectricityHelp()
    {
        nextStatus = "Box";
    }
    public void CallForAlarm(Vector3 Pos)
    {
        alarmPos = Pos;
        nextStatus = "Alarm";
    }
    public void EndHelp()
    {
        nextStatus = "Idle";
    }
 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerMovement>().GotCaught();
        }
    }
    private void FindTargetPlayer()
    {
        if(Vector3.Distance(transform.position,playerTarget.position)<viewDistance)
        {
            Vector3 dirToPlayer = (playerTarget.position-transform.position).normalized;
            if(Vector3.Angle((SCFovPos.transform.position - transform.position).normalized,dirToPlayer) < viewAngle / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, layerMask);
                if(raycastHit2D.collider != null)
                {
                    Debug.Log(raycastHit2D.collider.gameObject.name);
                    if (raycastHit2D.collider.gameObject.GetComponent<PlayerMovement>() != null)
                    {
                        alarmScript.ChaseStart();
                        nextStatus = "Chase";
                    }
                }
            }
        }
    }
    void FaceTarget() 
    {
        var vel = agent.velocity;
        vel.z = 0;
        if (vel != Vector3.zero) 
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vel);
        }
    }

}
