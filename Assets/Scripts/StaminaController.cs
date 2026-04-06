using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class StaminaController : MonoBehaviour
{
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float attackCost = 10;
    [SerializeField] private float jumpCost = 20;
    [HideInInspector] public bool isRunning = false;

    [SerializeField] private float runDrain = 10.0f;
    [SerializeField] private float staminaRegen = 15.0f;
    [SerializeField] private float regenDelay = 1.0f;

    private float regenerationTimer = 0f;

    void Start()
    {
        playerStamina = maxStamina;
    }

    void Update()
    {
        
        if(isRunning && playerStamina > 0)
        {
            playerStamina -= runDrain * Time.deltaTime;
            regenerationTimer = 0f;

            if (playerStamina <= 0)
            {
                playerStamina = 0;
                isRunning = false;
            }
        }
        else if(!isRunning && (playerStamina <= maxStamina - 0.01)){

            updateStamina();
        }

        if(isRunning && playerStamina <= 0)
        {
            isRunning = false;
        }
    }

    private void updateStamina()
    {
        regenerationTimer += Time.deltaTime;
        
        if(regenerationTimer >= regenDelay)
        {
            playerStamina += staminaRegen * Time.deltaTime;
        }
    }

    public void SetRun(bool running)
    {
        if(running && playerStamina > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public bool CanRun()
    {
        return playerStamina > 0;
    }
}
