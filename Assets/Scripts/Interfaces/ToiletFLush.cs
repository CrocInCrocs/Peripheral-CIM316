using UnityEngine;

public class ToiletFLush : MonoBehaviour, IInteractable
{
    [SerializeField] private SoundType Toilet;
    [SerializeField] private bool playAtObjectPosition = true;
    [SerializeField] private float volume = 1f;

    public void Interact()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("No SoundManager in the scene!");
            return;
        }

        if (playAtObjectPosition)
        {
            SoundManager.Instance.PlaySound(Toilet, transform.position, volume);
        }
        else
        {
            SoundManager.Instance.PlayGlobalSound(Toilet, volume);
        }
    }
}