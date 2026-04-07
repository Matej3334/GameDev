using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerMovementManager manager;
    private PlayerLook look;
    void Awake()
    {
        HideCursor();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        manager = GetComponent<PlayerMovementManager>();
        look = GetComponent<PlayerLook>();


        onFoot.Jump.performed += ctx => manager.Jump();
        onFoot.Run.started += ctx => manager.StartRun();
        onFoot.Run.canceled += ctx => manager.StopRun();
        onFoot.Attack.performed += ctx => manager.Attack();
        onFoot.Crouch.performed += ctx => manager.Crouch();
    }

    void FixedUpdate()
    {
        manager.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
