using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static GameObject player;

    private CharacterController characterController;
    private StaminaController staminaController;
    private Vector3 playerVelocity;
    public float speed;
    private float playerSpeed;
    public bool isRunning = false;
    public float runningSpeed;
    [HideInInspector] private bool isGrounded;

    public float jumpHeight = 3f;
    public float gravity = -9.8f;
    private Animator animator;

    void Awake()
    {
        player = gameObject;
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        staminaController = GetComponent<StaminaController>();
        animator = GetComponent<Animator>();
    }
    

    void Update()
    {
        isGrounded = characterController.isGrounded; 
    }

    public void ProcessMove(Vector2 input)
    {

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
        
        animator.SetFloat("XVel", input.x);
        animator.SetFloat("YVel", input.y);
        animator.SetBool("Wow", true);
        characterController.Move(playerVelocity * Time.deltaTime);

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
    }
}
