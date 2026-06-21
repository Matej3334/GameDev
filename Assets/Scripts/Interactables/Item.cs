using UnityEngine;

public class Item : Interact
{
    [SerializeField] private OpenDoor door;
    [SerializeField] private AudioClip clip;
    private AudioSource audioSource;
    private BoxCollider boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    protected override void Interacts()
    {
        base.Interacts();
        if (door != null)
        {
            door.SetKey();
        }
        audioSource.PlayOneShot(clip);

        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        boxCollider.enabled = false;
        //Destroy(gameObject);
    }
}
