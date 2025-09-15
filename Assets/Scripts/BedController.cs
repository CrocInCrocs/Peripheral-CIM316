using UnityEngine;

public class BedController : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        StartFade();
    }

    public void StartFade()
    {
        PeripheralGameManager.Current.StartSleep();
    }
}
