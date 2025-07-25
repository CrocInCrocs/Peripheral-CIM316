using UnityEngine;

public class SiginalDropSOund : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip soundToPlay;
    public Transform soundPosition;
    public float volume = 1f;

    [Header("Prefab Settings")]
    public GameObject audioPrefab; // Prefab with AudioSource

    public void PlaySoundAtPosition()
    {
        if (audioPrefab != null && soundToPlay != null && soundPosition != null)
        {
            GameObject audioInstance = Instantiate(audioPrefab, soundPosition.position, Quaternion.identity);
            AudioSource source = audioInstance.GetComponent<AudioSource>();

            if (source != null)
            {
                source.clip = soundToPlay;
                source.volume = volume;
                source.Play();
                Destroy(audioInstance, soundToPlay.length + 0.1f); // Cleanup after playing
            }
            else
            {
                Debug.LogWarning("AudioPrefab does not have an AudioSource component!");
            }
        }
        else
        {
            Debug.LogWarning("Missing references on SiginalDropSound!");
        }
    }
}