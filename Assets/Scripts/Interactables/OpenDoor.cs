using UnityEngine;

public class OpenDoor : Interact
{
    private Animator animator;
    [SerializeField] private bool canOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interacts()
    {
        if (canOpen)
        {
            base.Interacts();
            Debug.Log("interact");
            if (!animator.GetBool("Opened"))
            {
                animator.Play("Open");
                animator.SetBool("Opened", true);
            }
            else
            {
                animator.SetBool("Opened", false);
                animator.Play("Close");
            }
        } else
        {
            Debug.Log("Can't Open");
        }
    }

    public void SetKey()
    {
        canOpen = true;
    }
}
