using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup WinScreen;
    private AudioSource audioSource;

    void Start()
    {
        WinScreen.alpha = 0f;
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider otherCollider = other.GetComponent<Collider>();
        if (otherCollider != null)
        {
            StartCoroutine(WinCouroutine());
            StartCoroutine(FreezeTime());
        }


    }

    IEnumerator WinCouroutine()
    {
        float elapsed = 0;
        while (elapsed < 2f)
        {
            elapsed += Time.deltaTime;
            WinScreen.alpha = Mathf.Clamp01(elapsed / 2f);
            yield return null;
        }
    }

    IEnumerator FreezeTime()
    {
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
    }
}
