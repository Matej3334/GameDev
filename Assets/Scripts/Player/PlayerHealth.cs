using System.Collections;
using TMPro;
using UnityEngine;
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
    private AudioSource audioSource;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerAnimator = GetComponent<Animator>();
        currentHP = MaxHP;
        DeathScreen.alpha= 0f;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float transparency = 1f - (currentHP/100f);
        Color imageColor = Color.white;
        imageColor.a = transparency;
        HealthColor.color = imageColor;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log(currentHP + " - Current HP");
        if (currentHP <= 0 && !GameDone)
        {
            GameDone = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        audioSource.PlayOneShot(deathClip);
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
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
    }
}
