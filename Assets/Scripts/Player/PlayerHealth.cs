using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float MaxHP = 100f;
    private float currentHP;
    [SerializeField] private Image HealthColor;
    [SerializeField] private Canvas Crosshair;
    private Animator playerAnimator;
    private InputManager inputManager;
    [SerializeField] private CanvasGroup DeathScreen;
    private bool GameDone = false;

    [SerializeField] public AudioClip deathClip;
    [SerializeField] public AudioClip takeDamageClip;
    private AudioSource audioSource;
    private float transparency;
    private float lastTransparency= 1f;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerAnimator = GetComponent<Animator>();
        currentHP = MaxHP;
        DeathScreen.alpha= 0f;
        audioSource = GetComponent<AudioSource>();

        transparency = 1f;
        Color imageColor = Color.white;
        imageColor.a = transparency;
        HealthColor.color = imageColor;
    }

    void Update()
    {
        transparency = 1f - (currentHP/100f);
        if (Mathf.Abs(lastTransparency - transparency) > 0.01f)
        {
            lastTransparency = transparency;
            Color imageColor = Color.white;
            imageColor.a = transparency;
            HealthColor.color = imageColor;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!GameDone)
        {
            audioSource.PlayOneShot(takeDamageClip);
            audioSource.pitch = 1f;
            currentHP -= damage;
            Debug.Log(currentHP + " - Current HP");
        }
        if (currentHP <= 0 && !GameDone)
        {
            GameDone = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        inputManager.OnFootDisable();
        playerAnimator.SetTrigger("Death");
        StartCoroutine(DeathCouroutine());
        StartCoroutine(FreezeTime());
    }
    
    IEnumerator DeathCouroutine()
    {
        Crosshair.enabled = false;
        HealthColor.enabled = false;

        float elapsed = 0;
        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;
            DeathScreen.alpha = Mathf.Clamp01(elapsed / 2f);
            yield return null;
        }
    }

    IEnumerator FreezeTime()
    {
        audioSource.PlayOneShot(deathClip);
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
    }
}
