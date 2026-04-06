using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    //Message to player
    public string MessagePrompt;
    
    public void BaseInteract()
    {
        Interacts();
    }
    protected virtual void Interacts()
    {
        //override this function when player interacts with object
    }
}
