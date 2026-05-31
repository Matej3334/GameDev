using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class PlayerMovementManager : MonoBehaviour
{
    public static GameObject player;
    [SerializeField] private float AnimationBlendSpeed = 0.2f;
    private CharacterController characterController;
    private StaminaController staminaController;

    private Vector3 playerVelocity;
    private Vector3 currentDirection = Vector3.zero;
    public float speed;
    private float playerSpeed;
    public bool isRunning = false;
    public bool canAttack = true;
    public float runningSpeed;
    public float crouchSpeed = 2.5f;
    [HideInInspector] private bool isGrounded;
    private float noMove = 0f;
    private bool isCrouched = false;
    private float JumpDelay = 10f;
    private float JumpCooldown;

    public float jumpHeight = 3f;
    public float gravity = -9.8f;
    private Animator animator;
    
    public Transform groundCheck;
    public float groundDistance = 0.05f;
    public LayerMask groundMask = 7;

    private int ActiveWeapon = 0;
    [SerializeField] private Transform WeaponSpawn;
    private GameObject currentWeapon = null;
    private WeaponAttack weaponAttack = null;
    private int WeaponDurability;

    [SerializeField] public AudioClip attackClip;
    [SerializeField] public AudioClip footstepClip;
    private AudioSource audioSource;
    private float footstepTimer;
    private float footstepPause = 0.6f; 

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
        staminaController = GetComponent<StaminaController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }
    

    public void ProcessMove(Vector2 input)
    {
        if (noMove < 0)
        {
            if (!isCrouched)
            {
                isRunning = staminaController.isRun();
                playerSpeed = isRunning ? runningSpeed : speed;
            }
            else
            {
                playerSpeed = crouchSpeed;
            }

            currentDirection.x = Mathf.Lerp(currentDirection.x, input.x * playerSpeed, AnimationBlendSpeed * Time.deltaTime);
            currentDirection.z = Mathf.Lerp(currentDirection.z, input.y * playerSpeed, AnimationBlendSpeed * Time.deltaTime);

            characterController.Move(transform.TransformDirection(currentDirection) * Time.deltaTime);
            playerVelocity.y += gravity * Time.deltaTime;

            if (isGrounded && (currentDirection.magnitude > 2.5f))
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer < 0)
                {
                    audioSource.PlayOneShot(footstepClip);
                    audioSource.pitch = 1.5f;
                    if (isCrouched)
                    {
                        audioSource.volume = 0.25f;
                    }
                    else
                    {
                        audioSource.volume = 0.5f;
                    }
                    if (isRunning)
                    {
                        footstepTimer = footstepPause * 0.6f;
                    }
                    else
                    {
                        footstepTimer = footstepPause;
                    }
                }
            }
            else
            {
                footstepTimer = 0;
            }


            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            
             animator.SetFloat("XVel", currentDirection.x);
             animator.SetFloat("YVel", currentDirection.z);
            

            characterController.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            noMove -= Time.deltaTime;
            animator.SetFloat("XVel", 0);
            animator.SetFloat("YVel", 0);
        }

        if (JumpCooldown > 0)
        {
            JumpCooldown -= 5.0f * Time.deltaTime;
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
        if (isGrounded && !isCrouched && staminaController.CanJump() && JumpCooldown<=0)
        {
            JumpCooldown = JumpDelay;
            staminaController.SetJump();
            animator.SetBool("Jump", true);
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            isGrounded = false;
            canAttack = false;
            characterController.center = new Vector3(0, 1.3f, 0);
            characterController.height = 2.6f;
            StartCoroutine(SizeCoroutine());
        }
    }

    IEnumerator SizeCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        characterController.center = new Vector3(0, 1f, 0);
        characterController.height = 2f;
    }


    public void StartRun()
    {
        if (isGrounded && staminaController.CanRun() && !isCrouched) 
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

        if (isGrounded && canAttack && !(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack") && !isCrouched && staminaController.canAttack() && ActiveWeapon != 0)
        {
            WeaponDurability = weaponAttack.Attack();
            if (WeaponDurability <= 0)
            {
                Destroy(currentWeapon);
                currentWeapon = null;
                ActiveWeapon = 0;
                animator.SetTrigger("Break");
            }
            else {
                animator.SetTrigger("Attack");
                audioSource.PlayOneShot(attackClip);
                audioSource.pitch = 1f;
                staminaController.SetAttack();
                canAttack = false;
                noMove = 1.5f;
            }
        }
    }

    public void Crouch()
    {
        if (!isCrouched)
        {
            isCrouched = true;
            animator.SetBool("Crouch", true);
            characterController.center = new Vector3(0, 0.6f, 0);
            characterController.height = 1.2f;
            characterController.radius = 0.7f;
        }
        else
        {
            isCrouched = false;
            animator.SetBool("Crouch", false);
            characterController.height = 2f;
            characterController.center = new Vector3(0, 1f, 0);
            characterController.radius = 0.4f;
        }
    }

    public void SetWeapon(GameObject weapon, int num)
    {
        if (ActiveWeapon != 0)
        {
            Destroy(currentWeapon);
        }
        ActiveWeapon = num;
        currentWeapon=Instantiate(weapon, WeaponSpawn);
        weaponAttack = currentWeapon.GetComponent<WeaponAttack>();
    }

}
