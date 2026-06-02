using UnityEngine;

public class Scream : MonoBehaviour
{
    new AudioSource audio;
    BoxCollider boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
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
