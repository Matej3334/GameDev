using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AllKnowingAlgorithm : MonoBehaviour
{

    private NavMeshAgent agent;
    private NavMeshPath path;
    private Animator EnemyAnim;
    private bool isAttacking = false;
    [SerializeField] private EnemyAttack enemyAttack;
    private NavMeshQueryFilter filter;
    private float distance;
    private Transform target;

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
            distance = Vector3.Distance(target.position, transform.position);

            if (distance < agent.stoppingDistance && !isAttacking)
            {
                enemyAttack.Attack();
                isAttacking = true;
                EnemyAnim.SetTrigger("attack");
                StartCoroutine(AttackCoroutine());
            }
            else if (!isAttacking)
            {
                NavMesh.CalculatePath(transform.position, target.position, filter, path);
                agent.SetPath(path);
                EnemyAnim.SetBool("walk", true);
            }
        }
    }


    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }
}
