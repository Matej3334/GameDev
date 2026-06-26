using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private InputManager inputManager;
    private PlayerUI playerUI;
    private AudioSource audioSource;
    private float lastRaycastTime = 0f;
    private float raycastInterval = 0.1f;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerUI = GetComponent<PlayerUI>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerCamera != null)
        {
            playerUI.UpdateText(string.Empty);
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
            {
                if (hit.collider.GetComponent<Interact>() != null)
                {
                    Interact interactable = hit.collider.GetComponent<Interact>();
                    playerUI.UpdateText(interactable.MessagePrompt);
                    if (inputManager.onFoot.Interact.triggered)
                    {
                        interactable.BaseInteract();
                    }
                }
            }

        }
    }

    void OnDestroy()
    {
        audioSource.enabled = false;
    }
}