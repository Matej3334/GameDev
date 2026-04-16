using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {

        Camera currentCamera = Camera.main;
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(currentCamera.transform.position, currentCamera.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance, mask))
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
