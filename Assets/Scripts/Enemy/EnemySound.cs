using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip attackingClip;
   
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void WalkSound()
    {
        audioSource.PlayOneShot(walkingClip);
    }

    public void AttackSound()
    {
        audioSource.PlayOneShot(attackingClip);
    }
}
