using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementManager : MonoBehaviour
{
    public static GameObject player;
    [SerializeField] private float AnimationBlendSpeed = 8.9f;
    private CharacterController characterController;
    private StaminaController staminaController;

    private Vector3 playerVelocity;
    public float speed;
    private float playerSpeed;
    public bool isRunning = false;
    public bool canAttack = true;
    public float runningSpeed;
    [HideInInspector] private bool isGrounded;
    private float noMove = 0f;

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
        if (noMove < 0)
        {
            isRunning = staminaController.isRun();
            playerSpeed = isRunning ? runningSpeed : speed;

            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = Mathf.Lerp(moveDirection.x, input.x * playerSpeed, AnimationBlendSpeed);
            moveDirection.z = Mathf.Lerp(moveDirection.z, input.y * playerSpeed, AnimationBlendSpeed);

            characterController.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);
            playerVelocity.y += gravity * Time.deltaTime;

            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            if (isRunning)
            {
                animator.SetFloat("XVel", moveDirection.x);
                animator.SetFloat("YVel", moveDirection.z);
            }
            else
            {
                animator.SetFloat("XVel", moveDirection.x);
                animator.SetFloat("YVel", moveDirection.z);
            }

            characterController.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            noMove -= Time.deltaTime;
            animator.SetFloat("XVel", 0);
            animator.SetFloat("YVel", 0);
        }
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
            if (animator.GetBool("Jump")==true)
            {
                animator.SetBool("Jump", false);
            }
            playerVelocity.y = -2f;

            canAttack = true;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            animator.SetBool("Jump", true);
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            isGrounded = false;
            canAttack = false;
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

    public void Attack()
    {
        if (isGrounded && canAttack && !(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack"))
        {
            animator.SetTrigger("Attack");
            canAttack = false;
            noMove = 1.5f;
        }
    }

}
