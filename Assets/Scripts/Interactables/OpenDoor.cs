using UnityEngine;

public class OpenDoor : Interact
{
    private Animator animator;
    [SerializeField] private bool canOpen;
    private AudioSource audioSource;
    private string defaultMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        defaultMessage = MessagePrompt;
    }


    protected override void Interacts()
    {
        if (canOpen)
        {
            MessagePrompt = defaultMessage;

            base.Interacts();
            Debug.Log("interact");
            audioSource.Play();
            if (!animator.GetBool("Opened"))
            {
                animator.Play("Open");
                animator.SetBool("Opened", true);
                canOpen = false;
            }
            else
            {
                animator.SetBool("Opened", false);
                animator.Play("Close");
                canOpen = false;
            }
        } else
        {
            MessagePrompt = "Can't Open Door";
            Debug.Log("Can't Open");
        }
    }

    public void SetKey()
    {
        canOpen = true;
        MessagePrompt = defaultMessage;
    }

    public void SetOpen()
    {
        canOpen = true;
        MessagePrompt = defaultMessage;
    }

}
