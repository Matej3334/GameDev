using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SimpleEnemy : MonoBehaviour
{
    public float lookRadius = 15f;
    private float viewAngle = 120f;
    private float distance;
    private Transform target;
    private NavMeshAgent agent;
    private NavMeshPath path;

    //[SerializeField] private LayerMask mask;
    private Animator EnemyAnim;
    private bool isAttacking = false;
    [SerializeField] private EnemyAttack enemyAttack;
    private NavMeshQueryFilter filter;
    private bool isChasing = false;

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
            if (DetectPlayer() || isChasing)
            {
                isChasing = true;
                MoveToPlayer();
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
            //FaceTarget();
            EnemyAnim.SetTrigger("attack");
            StartCoroutine(AttackCoroutine());
        }
        else if (distance > lookRadius || Physics.Linecast(transform.position, target.position))
        {
            isChasing = false;
            agent.SetDestination(transform.position);
            EnemyAnim.SetBool("walk", false);
        }
        else if (!isAttacking)
        {
            NavMesh.CalculatePath(transform.position, target.position, filter, path);
            agent.SetPath(path);
            EnemyAnim.SetBool("walk", true);
        }
    }


    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    private void FaceTarget()
    {
        transform.LookAt(target.transform);
    }

    private void LookAt(Vector3 newDirection)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), Time.deltaTime);
    }
}
