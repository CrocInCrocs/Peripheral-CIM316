using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour,IInteractable
{
    [SerializeField] private Renderer screenRenderer;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource tvAudio;
    [SerializeField] private Material screenOnMaterial;
    [SerializeField] private Material screenOffMaterial;

    private bool isOn = false;
    private double savedTime = 0;

    [SerializeField] private float fadeDuration = 1f;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (screenRenderer == null)
            screenRenderer = GetComponentInChildren<Renderer>();
        if (videoPlayer == null)
            videoPlayer = GetComponentInChildren<VideoPlayer>();
        if (tvAudio == null)
            tvAudio = GetComponentInChildren<AudioSource>();

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, tvAudio);

        screenRenderer.material = screenOffMaterial;
        videoPlayer.Pause();
        tvAudio.Pause();
        tvAudio.volume = 0f; // start muted
    }

    public void Interact()
    {
        isOn = !isOn;

        if (isOn)
        {
            Debug.Log("TV turned ON");
            screenRenderer.material = screenOnMaterial;

            videoPlayer.time = savedTime;
            videoPlayer.Play();

            tvAudio.time = (float)savedTime;
            tvAudio.Play();

            // // Start volume at 0; fade in only when player enters trigger
            // tvAudio.volume = 0f;

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            
            
        }
        else
        {
            Debug.Log("TV turned OFF");
            savedTime = videoPlayer.time;

            videoPlayer.Pause();
            tvAudio.Pause();

            screenRenderer.material = screenOffMaterial;

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            tvAudio.volume = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOn)
        {
            Debug.Log("Player ENTERED TV audio zone, fading volume UP");
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudioTo(1f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOn)
        {
            Debug.Log("Player LEFT TV audio zone, fading volume DOWN");
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudioTo(0f));
        }
    }

    private IEnumerator FadeAudioTo(float targetVolume)
    {
        float startVolume = tvAudio.volume;
        float elapsed = 0f;
        Debug.Log($"Starting fade from {startVolume} to {targetVolume}");

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tvAudio.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        tvAudio.volume = targetVolume;
        fadeCoroutine = null;
        Debug.Log($"Fade complete. Volume set to {targetVolume}");
    }
}