using UnityEngine;
using System.Collections;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] int damage = 10;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Attack()
    {
        Debug.Log("Attacking");
        currentDurability -= durabilityPerHit;
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
            if (collider.gameObject.CompareTag("Enemy"))
            {
                //Destroy(collider.gameObject);
                Debug.Log("Collision");
            }
        }
    }
}
