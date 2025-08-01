using UnityEngine;
using UnityEngine.Video;

public class ChannelSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] channels;
    private int currentChannelIndex = 0;

    public void Interact()
    {
        if (videoPlayer == null || channels.Length == 0)
        {
            Debug.LogWarning("Missing VideoPlayer or no channels assigned.");
            return;
        }

        // Cycle to next channel
        currentChannelIndex = (currentChannelIndex + 1) % channels.Length;
        videoPlayer.clip = channels[currentChannelIndex];

        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}