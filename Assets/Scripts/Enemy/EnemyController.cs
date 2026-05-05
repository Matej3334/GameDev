using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    private Transform target;
    private NavMeshAgent agent;
    private Animator EnemyAnim;
    private bool isAttacking = false;
    [SerializeField] private LayerMask mask;
    private bool DoorOpened = false;
    [SerializeField] private EnemyAttack enemyAttack;
    private float viewAngle = 90f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyAnim = GetComponent<Animator>();


        if (agent == null)
        {
            Debug.LogWarning("No NavMeshAgent, enemy will only face player");
        }

        if (PlayerMovementManager.player != null)
        {
            target = PlayerMovementManager.player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure PlayerMovementManager runs before EnemyController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance < lookRadius)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleBetween < viewAngle / 2)
            {
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
                if (agent.enabled)
                {
                    agent.isStopped = false;

                    if (distance <= agent.stoppingDistance && !isAttacking)
                    {
                        enemyAttack.Attack();
                        isAttacking = true;
                        FaceTarget();
                        EnemyAnim.SetTrigger("attack");
                        StartCoroutine(AttackCoroutine());

                    }
                    else if (path.status == NavMeshPathStatus.PathPartial)
                    {
                        agent.SetPath(path);
                        Ray ray = new Ray(transform.position, transform.forward);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 4f, mask))
                        {
                            if (hit.collider.GetComponent<Interact>() != null && !DoorOpened)
                            {
                                //agent.SetDestination(hit.collider.transform.position)

                                Interact interactable = hit.collider.GetComponent<Interact>();
                                interactable.BaseInteract();
                                DoorOpened = true;

                            }
                        }
                    }
                    else if (!isAttacking)
                    {
                        DoorOpened = false;
                        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
                        agent.SetPath(path);
                        EnemyAnim.SetBool("walk", true);
                    }
                }
            }
            else
            {
                if (agent.enabled)
                {
                    agent.isStopped = true;
                    EnemyAnim.SetBool("walk", false);
                }
            }
        }
    }

    void FaceTarget()
    {
        transform.LookAt(target.transform);
        //Vector3 direction = (transform.position - target.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void AttackOver()
    {
        Debug.Log("Attack Over");
        isAttacking=false;
    }


    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }
}
