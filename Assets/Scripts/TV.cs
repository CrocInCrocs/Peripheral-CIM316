using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour,IInteractable
{
    [SerializeField] private Renderer screenRenderer;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource tvAudio;
    [SerializeField] private GameObject blackOverlay;

    [SerializeField] private Collider audioAreaCollider;
    [SerializeField] private Transform playerTransform;

    private bool isOn = false;
    private double savedTime = 0;

    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float tvVolume = 5f;

    private float targetVolume = 0f;

    void Start()
    {
        if (screenRenderer == null) screenRenderer = GetComponentInChildren<Renderer>();
        if (videoPlayer == null) videoPlayer = GetComponentInChildren<VideoPlayer>();
        if (tvAudio == null) tvAudio = GetComponentInChildren<AudioSource>();

        if (audioAreaCollider == null) Debug.LogWarning("Audio area collider not assigned!");
        if (playerTransform == null) Debug.LogWarning("Player transform not assigned!");

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, tvAudio);

        videoPlayer.Pause();
        tvAudio.Pause();
        tvAudio.volume = 0f;
    }

    void Update()
    {
        if (!isOn || audioAreaCollider == null || playerTransform == null) return;

        bool inside = audioAreaCollider.bounds.Contains(playerTransform.position);
        targetVolume = inside ? tvVolume : 0f;

        // Smoothly fade volume
        tvAudio.volume = Mathf.MoveTowards(tvAudio.volume, targetVolume, fadeSpeed * Time.deltaTime);

        // Play/pause based on volume
        if (tvAudio.volume > 0f && !tvAudio.isPlaying) tvAudio.Play();
        else if (tvAudio.volume <= 0f && tvAudio.isPlaying) tvAudio.Pause();
    }

    public void Interact()
    {
        isOn = !isOn;

        if (isOn)
        {
            videoPlayer.time = savedTime;
            videoPlayer.Play();

            tvAudio.time = (float)savedTime;
            tvAudio.Play();
            tvAudio.volume = 0f;

            blackOverlay.SetActive(false);
        }
        else
        {
            savedTime = videoPlayer.time;

            videoPlayer.Pause();
            tvAudio.Pause();
            tvAudio.volume = 0f;

            blackOverlay.SetActive(true);
        }
    }
}