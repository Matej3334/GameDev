using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

public class SmartAlgorithm : MonoBehaviour
{
    private float lookRadius = 15f;
    private float viewAngle = 120f;
    private float distance;
    private Transform target;
    private NavMeshAgent agent;
    private NavMeshPath path;
    [SerializeField] private PlayerMovementManager playerManager;

    private Animator EnemyAnim;
    private bool isAttacking = false;
    [SerializeField] private EnemyAttack enemyAttack;
    private NavMeshQueryFilter filter;
    private bool isChasing = false;

    private bool WaypointSet = false;
    private Vector3 Waypoint;
    private Vector3 random;
    private bool PathSet = false;

    private float xPos;
    private float zPos;
    private float WaypointTimer = 0f;
    private bool playerLost = false;
    private float LookingForPlayerTimer = 0f;
    private Vector3 PlayersLastKnownPosition;

    void Start()
    {
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
            if (playerLost)
            {
                LookingForPlayerTimer -= Time.deltaTime;
                if(LookingForPlayerTimer <= 0f)
                {
                    playerLost = false;
                }
            }

            if (DetectPlayer() || isChasing)
            {
                playerLost = false;
                isChasing = true;
                MoveToPlayer();
            }
            else
            {
                RandomPatrol();
            }
        }
    }

    private bool DetectPlayer()
    {
        distance = Vector3.Distance(target.position, transform.position);
        float noiseLevel = playerManager.PlayerNoiseLevel / distance;

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
        
        if (noiseLevel > 1.5f)
        {
            Debug.Log("Noise");
            return true;
        }
        
         return false;
    }

    private void MoveToPlayer()
    {
        if (distance < agent.stoppingDistance && !isAttacking)
        {
            agent.SetDestination(transform.position);
            enemyAttack.Attack();
            isAttacking = true;
            EnemyAnim.SetTrigger("attack");
            StartCoroutine(AttackCoroutine());
        }
        else if (distance > lookRadius || Physics.Linecast(transform.position, target.position))
        {
            isChasing = false;
            //agent.SetDestination(transform.position);
            //EnemyAnim.SetBool("walk", false);
            PlayersLastKnownPosition = target.position;
            NavMesh.CalculatePath(transform.position, PlayersLastKnownPosition, filter, path);
            agent.SetPath(path);

            WaypointSet = false;
            playerLost = true;
            PathSet = false;
            LookingForPlayerTimer = 30f;
            
        }
        else if (!isAttacking)
        {
            NavMesh.CalculatePath(transform.position, target.position, filter, path);
            agent.SetPath(path);
            EnemyAnim.SetBool("walk", true);
        }
    }

    private void RandomPatrol()
    {
        if (!WaypointSet)
        {
            if (playerLost && LookingForPlayerTimer > 0)
            {
                Vector3 randomDir = Random.insideUnitSphere * 10f;
                randomDir.y = 0;
                random = PlayersLastKnownPosition + randomDir;
            }
            else if(LookingForPlayerTimer > 0) {
                playerLost = false;
            }
            else
            {
                random = new Vector3(Random.Range(0f, -60f), 0f, Random.Range(0f, 60f));
            }

            
            NavMeshHit hit;

            if(NavMesh.SamplePosition(random, out hit, 2f, filter))
            {
                Waypoint = hit.position;
                WaypointSet = true;
            }
        }
        else
        {
            if (!PathSet)
            {
                NavMesh.CalculatePath(transform.position, Waypoint, filter, path);
                agent.SetPath(path);
                EnemyAnim.SetBool("walk", true);
                PathSet = true;
            }
            else
            {
                xPos = Mathf.Pow(transform.position.x - Waypoint.x, 2);
                zPos = Mathf.Pow(transform.position.z - Waypoint.z, 2);

                if (xPos + zPos < 5f)
                {
                    EnemyAnim.SetBool("walk", false);
                    WaypointTimer += Time.deltaTime;
                    if (WaypointTimer > 2)
                    {
                        WaypointTimer = 0;
                        WaypointSet = false;
                        PathSet = false;
                    }
                }
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

}
