using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RoamingAlgorithm : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private Animator EnemyAnim;
    private bool isAttacking = false;
    [SerializeField] private EnemyAttack enemyAttack;
    private NavMeshQueryFilter filter;
    private float distance;
    private Transform target;

    [SerializeField] private Transform[] waypoints;
    private int currentWaypoint;
    private float WaypointTimer = 0f;

    private float lookRadius = 15f;
    private float viewAngle = 120f;
    private bool isChasing = false;
    private bool WaypointSet = false;

    private float xPos;
    private float zPos;

    void Start()
    {
        currentWaypoint = 0;

        agent = GetComponent<NavMeshAgent>();
        EnemyAnim = GetComponent<Animator>();

        path = new NavMeshPath();
        filter = new NavMeshQueryFilter();
        filter.agentTypeID = agent.agentTypeID;
        filter.areaMask = agent.areaMask;

        if (PlayerMovementManager.player != null)
        {
            target = PlayerMovementManager.player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure PlayerMovementManager runs before EnemyController.");
        }
    }

    void Update()
    {
        if (agent.enabled)
        {
            if (DetectPlayer() || isChasing)
            {
                isChasing = true;
                MoveToPlayer();
            }
            else
            {
                MoveToWaypoint();
            }
        }
    }


    private bool DetectPlayer()
    {
        distance = Vector3.Distance(target.position, transform.position);

        if (distance < lookRadius)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetween < viewAngle / 2)
            {
                if (!Physics.Linecast(transform.position, target.position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MoveToPlayer()
    {
        if (distance < agent.stoppingDistance && !isAttacking)
        {
            enemyAttack.Attack();
            isAttacking = true;
            EnemyAnim.SetTrigger("attack");
            StartCoroutine(AttackCoroutine());
        }
        else if (distance > lookRadius || Physics.Linecast(transform.position, target.position))
        {
            isChasing = false;
            agent.SetDestination(transform.position);
            EnemyAnim.SetBool("walk", false);
            WaypointSet = false;
        }
        else if (!isAttacking)
        {
            NavMesh.CalculatePath(transform.position, target.position, filter, path);
            agent.SetPath(path);
            EnemyAnim.SetBool("walk", true);
        }
    }

    private void MoveToWaypoint()
    {
        xPos = Mathf.Pow(transform.position.x - waypoints[currentWaypoint].position.x, 2);
        zPos = Mathf.Pow(transform.position.z - waypoints[currentWaypoint].position.z, 2);
        
        if (xPos + zPos < 5f)
        {
            EnemyAnim.SetBool("walk", false);
            WaypointTimer += Time.deltaTime;
            if (WaypointTimer > 2)
            {
                if (currentWaypoint == waypoints.Length - 1)
                {
                    currentWaypoint = 0;
                }
                else
                {
                    currentWaypoint++;
                }
                WaypointTimer = 0;
                WaypointSet = false;
            }
        }

        if (!WaypointSet)
        {
            NavMesh.CalculatePath(transform.position, waypoints[currentWaypoint].position, filter, path);
            agent.SetPath(path);
            EnemyAnim.SetBool("walk", true);
            WaypointSet = true;
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }
}
