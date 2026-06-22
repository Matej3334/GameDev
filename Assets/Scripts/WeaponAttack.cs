using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] int durabilityPerHit = 100;
    [SerializeField] int maxDurability = 100;
    private BoxCollider collider;
    private int currentDurability;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        currentDurability = maxDurability;
    }

    public int Attack()
    {
        Debug.Log("Attacking");

        if (!(currentDurability <= 0))
        {
            collider.enabled = true;
            StartCoroutine(ColliderCoroutine());
        }
        return currentDurability;
    }

    IEnumerator ColliderCoroutine()
    {
        yield return new WaitForSeconds(1f);
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.enabled)
        {
            GameObject hitObject = collider.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hitObject.GetComponentInParent<EnemyHealth>();
                enemy.Damage(damage);
                //Destroy(collider.gameObject);
                currentDurability -= durabilityPerHit;
                Debug.Log(currentDurability);
                
            }
        }
    }
}
