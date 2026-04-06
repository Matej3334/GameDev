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
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        manager = GetComponent<PlayerMovementManager>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => manager.Jump();
        onFoot.Run.started += ctx => manager.StartRun();
        onFoot.Run.canceled += ctx => manager.StopRun();
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
}
