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
    
    public Transform groundCheck;
    public float groundDistance = 0.05f;
    public LayerMask groundMask = 7;

    void Awake()
    {
        player = gameObject;
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -0.1f, 0);
            groundCheck = groundCheckObj.transform;
        }
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        staminaController = GetComponent<StaminaController>();
        animator = GetComponent<Animator>();
    }
    

    public void ProcessMove(Vector2 input)
    {

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

        if (isRunning)
        {
            Debug.Log(input.x * 2 + "    " + input.y * 2);
            animator.SetFloat("XVel", input.x*2);
            animator.SetFloat("YVel", input.y*2);
        }
        else
        {
            animator.SetFloat("XVel", input.x);
            animator.SetFloat("YVel", input.y);
        }

            characterController.Move(playerVelocity * Time.deltaTime);

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        else if (playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

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
            isRunning = true;
            staminaController.SetRun(true);
        }
    }

    public void StopRun()
    {
        isRunning = false;
        staminaController.SetRun(false);
    }
}
