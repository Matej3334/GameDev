using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static GameObject player;

    private CharacterController characterController;
    private StaminaController staminaController;
    private Animator animator;
    private Vector3 playerVelocity;
    public float speed;
    private float playerSpeed;
    public bool isRunning = false;
    public float runningSpeed;
    [HideInInspector] private bool isGrounded;
    public float jumpHeight = 3f;
    public float gravity = -9.8f;
    public bool isWalking = false;
    
    private Vector2 currentInput;
    void Awake()
    {
        player = gameObject;
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        staminaController = GetComponent<StaminaController>();
    }
    

    void Update()
    {
        isGrounded = characterController.isGrounded; 
    }

    public void ProcessMove(Vector2 input)
    {
        currentInput = input;

        isRunning = staminaController.isRunning;
        playerSpeed = isRunning ? runningSpeed : speed;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        characterController.Move(transform.TransformDirection(moveDirection) * playerSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        characterController.Move(playerVelocity * Time.deltaTime);

        bool walk = currentInput.magnitude > 0.01f;
        
        if (isWalking != walk)
        {
            isWalking = walk;
            animator.SetBool("isWalking", walk);
            animator.SetFloat("speed", currentInput.magnitude);
        }
        
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void StartRun()
    {
        if (isGrounded && staminaController.CanRun()) 
        {
            staminaController.SetRun(true);
        }
    }

    public void StopRun()
    {
        staminaController.SetRun(false);
        animator.SetBool("isRunning", false);
    }
}
