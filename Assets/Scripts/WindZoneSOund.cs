using UnityEngine;

public class WindZoneSOund : MonoBehaviour
{
    [Header("Wind Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float indoorVolume = 0.01f;
    [SerializeField, Range(0f, 1f)] private float outdoorVolume = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.SetWindVolume(indoorVolume);
            Debug.Log($"🏠 Player entered indoor zone: wind volume set to {indoorVolume}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.SetWindVolume(outdoorVolume);
            Debug.Log($"🌬️ Player exited indoor zone: wind volume set to {outdoorVolume}");
        }
    }
}