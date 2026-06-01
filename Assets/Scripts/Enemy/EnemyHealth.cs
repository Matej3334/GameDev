using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int MaxHP = 100;
    private int currentHP;
    private Animator EnemyAnim;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private AudioSource audioSource;
    [SerializeField] private AudioClip takeDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = MaxHP;
        EnemyAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            audioSource.PlayOneShot(takeDamage);
            EnemyAnim.SetTrigger("Dead");
            agent.isStopped = true;
            agent.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
            StartCoroutine(KillEnemy());
        }
        else
        {
            audioSource.PlayOneShot(takeDamage);
            EnemyAnim.SetTrigger("takeDmg");
            EnemyAnim.SetBool("walk", false);
        }
        Debug.Log("EnemyHP" + currentHP);
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
