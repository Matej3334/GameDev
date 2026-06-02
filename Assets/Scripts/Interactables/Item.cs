using UnityEngine;

public class Item : Interact
{
    [SerializeField] private OpenDoor door;
    [SerializeField] private AudioClip clip;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        //Destroy(gameObject);
    }
}
