using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class StaminaController : MonoBehaviour
{
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float attackCost = 10f;
    [SerializeField] private float jumpCost = 30f;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool Jump = false;

    [SerializeField] private float runDrain = 20.0f;
    [SerializeField] private float staminaRegen = 25.0f;
    [SerializeField] private float regenDelay = 10.0f;

    private float regenerationTimer = 0f;

    void Start()
    {
        playerStamina = maxStamina;
    }

    void Update()
    {
        Debug.Log("Jump:" + Jump + " Stamina" + playerStamina);
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

        if (Jump)
        {
            playerStamina -= jumpCost;
            Jump = false;
        }
        if (isRunning && playerStamina <= 0)
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

    public bool isRun()
    {
        return isRunning;
    }

    public bool CanJump()
    {
        return playerStamina > jumpCost;
    }

    public void SetJump()
    {
        Jump = true;
    }

}
