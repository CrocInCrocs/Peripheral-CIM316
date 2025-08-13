using UnityEngine;

public class TVRemote : MonoBehaviour, IInteractable
{
    [SerializeField] private TV linkedTV; // Drag your TV GameObject here in Inspector

    public void Interact()
    {
        if (linkedTV != null)
        {
            linkedTV.Interact(); // Calls the TV's on/off logic
        }
    }
}