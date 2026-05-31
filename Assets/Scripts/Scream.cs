using UnityEngine;

public class Scream : MonoBehaviour
{
    AudioSource audio;
    BoxCollider boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider otherCollider = other.GetComponent<Collider>();
        if (otherCollider != null)
        {
            audio.Play();
            boxCollider.enabled = false;
        }


    }
}
