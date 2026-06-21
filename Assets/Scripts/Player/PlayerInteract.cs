using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Performance Settings")]
    [SerializeField] private float raycastInterval = 0.1f;
    [SerializeField] private bool useNonAlloc = true;
    [SerializeField] private int maxHits = 5;

    // Cached references
    private RaycastHit[] hitBuffer;
    private InputManager inputManager;
    private PlayerUI playerUI;
    private Interact cachedInteractable;
    private GameObject lastHitObject;
    private AudioSource audioSource;
    private float lastRaycastTime = 0f;
    private bool hasValidHit = false;

    private Ray reusableRay;

    private string lastDisplayedText = string.Empty;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerUI = GetComponent<PlayerUI>();
        audioSource = GetComponent<AudioSource>();

        if (useNonAlloc)
        {
            hitBuffer = new RaycastHit[maxHits];
        }

        reusableRay = new Ray();
    }

    void Update()
    {
        if (playerCamera != null)
        {
            if (Time.time - lastRaycastTime >= raycastInterval)
            {
                lastRaycastTime = Time.time;
                PerformOptimizedRaycast();
            }

            UpdateInteractionUI();
        }
    }

    private void PerformOptimizedRaycast()
    {
        reusableRay.origin = playerCamera.transform.position;
        reusableRay.direction = playerCamera.transform.forward;

        bool hitSomething = false;

        if (useNonAlloc)
        {
            int hitCount = Physics.RaycastNonAlloc(
                reusableRay,
                hitBuffer,
                interactionDistance,
                interactableLayer
            );

            if (hitCount > 0)
            {
                RaycastHit hit = hitBuffer[0];
                if (hit.collider != null)
                {
                    ProcessHit(hit);
                    hitSomething = true;
                }
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(reusableRay, out hit, interactionDistance, interactableLayer))
            {
                ProcessHit(hit);
                hitSomething = true;
            }
        }

        if (!hitSomething)
        {
            ClearCachedHit();
        }
    }

    private void ProcessHit(RaycastHit hit)
    {
        GameObject hitObject = hit.collider.gameObject;

        if (hitObject != lastHitObject)
        {
            if (!hit.collider.TryGetComponent<Interact>(out cachedInteractable))
            {
                cachedInteractable = null;
                lastHitObject = null;
                hasValidHit = false;
                return;
            }

            lastHitObject = hitObject;
            hasValidHit = true;
        }
        else if (cachedInteractable == null)
        {
            ClearCachedHit();
        }
    }

    private void ClearCachedHit()
    {
        cachedInteractable = null;
        lastHitObject = null;
        hasValidHit = false;

        UpdateUIText(string.Empty);
    }

    private void UpdateInteractionUI()
    {
        if (hasValidHit && cachedInteractable != null)
        {
            UpdateUIText(cachedInteractable.MessagePrompt);

            if (inputManager.onFoot.Interact.triggered)
            {
                cachedInteractable.BaseInteract();
            }
        }
        else
        {
            UpdateUIText(string.Empty);
        }
    }

    private void UpdateUIText(string newText)
    {
        if (lastDisplayedText != newText)
        {
            lastDisplayedText = newText;
            playerUI.UpdateText(newText);
        }
    }

    void OnDestroy()
    {
        hitBuffer = null;
        audioSource.enabled = false;
    }
}