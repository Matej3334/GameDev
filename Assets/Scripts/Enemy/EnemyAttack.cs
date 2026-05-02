using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    private BoxCollider weaponCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    public void Attack()
    {
        Debug.Log("Enemy Attacking");

        weaponCollider.enabled = true;
        StartCoroutine(ColliderCoroutine());

    }

    IEnumerator ColliderCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (weaponCollider.enabled)
        {
            GameObject hitObject = collider.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                PlayerHealth player = hitObject.GetComponentInParent<PlayerHealth>();
                player.TakeDamage(damage);
            }
        }
    }
}
