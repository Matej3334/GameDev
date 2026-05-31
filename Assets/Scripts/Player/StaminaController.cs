using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using System.Collections;


public class StaminaController : MonoBehaviour
{
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float attackCost = 40f;
    [SerializeField] private float jumpCost = 30f;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool Jump = false;

    [SerializeField] private float runDrain = 10.0f;
    [SerializeField] private float staminaRegen = 10.0f;
    [SerializeField] private float regenDelay = 2.0f;

    private bool Attack = false;
    private bool Action = false;
    private float regenerationTimer = 0f;
    private AudioSource audioSource;
    [SerializeField] private AudioClip breathing;
    private float breathingTimer = 3.0f;

    void Start()
    {
        playerStamina = maxStamina;
        audioSource = GetComponent<AudioSource>();
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
            if (!Action)
            {
                updateStamina();
            }
            else
            {
                StartCoroutine(ActionCoroutine());
            }
        }



        if (Jump)
        {
            playerStamina -= jumpCost;
            Jump = false;
        }

        else if(Attack){

            playerStamina -= attackCost;
            Attack = false;
        }

        if (isRunning && playerStamina <= 0)
        {
            isRunning = false;
        }

        breathingTimer-= Time.deltaTime;
        if (playerStamina <= 30 && breathingTimer < 0)
        {
            breathingTimer = 3.0f;
            audioSource.PlayOneShot(breathing);
        }
    }

    IEnumerator ActionCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        Action = false;

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
        Action = true;
    }

    public void SetAttack()
    {
        Attack = true;
        Action = true;
    }

    public bool canAttack()
    {
        return playerStamina > attackCost;
    }
}
