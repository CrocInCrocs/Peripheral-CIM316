using UnityEngine;

public class BedController : MonoBehaviour, IInteractable
{
    public GameObject nightDialogue;
    public void Interact()
    {
        if (PeripheralGameManager.Current.allChoresDone)
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        PeripheralGameManager.Current.StartSleep();
        PeripheralGameManager.Current.EnableTriggerBoxes();
        PeripheralGameManager.Current.LightsOut();
        nightDialogue.SetActive(true);
    }
}
