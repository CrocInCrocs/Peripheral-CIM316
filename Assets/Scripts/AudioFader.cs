using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFader : MonoBehaviour
{
    [Header("Audio Fader Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Collider audioAreaCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float stopThreshold = 0.01f; // volume at which we stop

    private void Reset()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (playerTransform == null && Camera.main != null)
            playerTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (audioSource == null || audioAreaCollider == null || playerTransform == null)
            return;

        bool inside = audioAreaCollider.bounds.Contains(playerTransform.position);
        float targetVolume = inside ? maxVolume : 0f;

        // Fade volume
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);

        // Control play/stop
        if (inside)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.volume <= stopThreshold && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}