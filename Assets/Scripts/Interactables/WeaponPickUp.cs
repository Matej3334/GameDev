using UnityEngine;

public class WeaponPickUp : Interact
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interacts()
    {
        base.Interacts();
        Destroy(gameObject);
    }

}
