using UnityEngine;

public class LightSwiitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light targetLight; // Assign your spotlight here
    [SerializeField] private bool startOn = false;

    private void Start()
    {
        if (targetLight != null)
            targetLight.enabled = startOn;
    }

    public void Interact()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }
    }
}