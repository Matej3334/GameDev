using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float MaxHP = 100f;
    private float currentHP;
    [SerializeField] private Image HealthColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = MaxHP;
    }

    // Update is called once per frame
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
        if (currentHP <= 0)
        {
            //Game Over
        }
    }
}
