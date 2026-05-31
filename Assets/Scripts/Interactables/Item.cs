using UnityEngine;

public class Item : Interact
{
    [SerializeField] private OpenDoor door;
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
        if (door != null)
        {
            door.SetKey();
        }

        Destroy(gameObject);
    }
}
