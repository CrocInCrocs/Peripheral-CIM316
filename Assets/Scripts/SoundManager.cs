using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Clips")] public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;

    [Header("Footstep Sounds")] public AudioClip[] footstepClips;

    [Header("Curtain Sounds")] public AudioClip curtainOpenClip;
    public AudioClip curtainCloseClip;

    [Header("Printer Sounds")] public AudioClip printerClip;


    [Header("Cat Food Sounds")] public AudioClip catfoodClip;

    [Header("Computer / CCTV Sounds")] public AudioClip computerOnClip;
    public AudioClip computerOffClip;
    public AudioClip cctvViewClip;
    public AudioClip SwitchCameraClip;

    [Header("Sink Sounds")]
    public AudioClip sinkOnClip;
    public AudioClip sinkOffClip;
    private AudioSource sinkAudioSource;
    

    [Header("Wind Sounds")]
    [SerializeField] private AudioClip windClip;
    [SerializeField] private AudioSource windAudioSource;

    private AudioSource rainAudioSource;
    public AudioClip rainOnClip;
    public AudioClip rainOffClip;

    public AudioClip ThunderClip;

    [Header("Audio Settings")] public AudioSource audioSourcePrefab;

    // Dedicated AudioSource for CCTV loop sound
    private AudioSource cctvAudioSource;

    private void Awake()
    {
        // Basic Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null || audioSourcePrefab == null) return;

        // Instantiate the AudioSource prefab at position
        AudioSource source = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        source.clip = clip;
        source.Play();

        // Destroy the AudioSource game object after clip length
        Destroy(source.gameObject, clip.length);
    }

    public void PlayDoorSound(bool opening, Vector3 position)
    {
        AudioClip clipToPlay = opening ? doorOpenClip : doorCloseClip;
        PlaySoundAtPosition(clipToPlay, position);
    }

    public void PlayFootstepSound(Vector3 position)
    {
        if (footstepClips.Length == 0 || audioSourcePrefab == null) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        PlaySoundAtPosition(clip, position);
    }

    public void PlayCurtainSound(bool closing, Vector3 position)
    {
        AudioClip clipToPlay = closing ? curtainCloseClip : curtainOpenClip;
        PlaySoundAtPosition(clipToPlay, position);
    }

    

    
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    public void PlaySoundGlobal(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 0f; // 2D sound
        source.Play();
        Destroy(source, clip.length);
    }
    
    public void PlayPrinterSound(Vector3 position)
    {
        PlaySoundAtPosition(printerClip, position);
    }

    public void PlayComputerOnSound(Vector3 position)
    {
        PlaySoundAtPosition(computerOnClip, position);
    }

    public void PlayComputerOffSound(Vector3 position)
    {
        PlaySoundAtPosition(computerOffClip, position);
    }
    // CCTV sound play (looping)


    public void SwitchCameraSound(Vector3 position)
    {
        PlaySoundAtPosition(SwitchCameraClip, position);
    }

    public void PlayCCTVLoopSound(Vector3 position)
    {
        if (cctvAudioSource == null && audioSourcePrefab != null && cctvViewClip != null)
        {
            cctvAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
            cctvAudioSource.clip = cctvViewClip;
            cctvAudioSource.loop = true;
            cctvAudioSource.Play();
            Debug.Log("CCTV sound playing");
        }
    }

    public void StopCCTVLoopSound()
    {
        if (cctvAudioSource != null)
        {
            cctvAudioSource.Stop();
            Destroy(cctvAudioSource.gameObject);
            cctvAudioSource = null;
            Debug.Log("CCTV sound stopped");
        }
    }


    public void StartSinkLoopSound(Vector3 position)
    {
        if (sinkAudioSource == null && audioSourcePrefab != null && sinkOnClip != null)
        {
            sinkAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
            sinkAudioSource.clip = sinkOnClip;
            sinkAudioSource.loop = true;
            sinkAudioSource.Play();
            Debug.Log("Sink loop sound playing");
        }
    }

    public void StopSinkLoopSound()
    {
        if (sinkAudioSource != null)
        {
            sinkAudioSource.Stop();
            Destroy(sinkAudioSource.gameObject);
            sinkAudioSource = null;
            Debug.Log("Sink loop sound stopped");
        }
    }


    // public void StartRainLoopSound()
    // {
    //     if (rainAudioSource == null && rainOnClip != null)
    //     {
    //         // Create a new AudioSource on the SoundManager (or use an existing one)
    //         rainAudioSource = gameObject.AddComponent<AudioSource>();
    //         rainAudioSource.clip = rainOnClip;
    //         rainAudioSource.loop = true;
    //         rainAudioSource.spatialBlend = 0f; // 2D sound
    //         rainAudioSource.Play();
    //         Debug.Log("Rain sound playing as 2D");
    //     }
    // }
    public void StartRainLoopSound(Vector3 position) // from position
    {
        if (rainAudioSource == null && audioSourcePrefab != null && rainOnClip != null)
        {
            // Raise the sound by, for example, 10 units
            position.y += 10f;
            rainAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
            rainAudioSource.clip = rainOnClip;
            rainAudioSource.loop = true;
            rainAudioSource.Play();
            Debug.Log("Rain sound playing");
        }
    }

    public void PlayThunderSound(Vector3 position)
    {
        PlaySoundAtPosition(ThunderClip, position);
    }

    public void StopRainLoopSound()
    {
        if (rainAudioSource != null)
        {
            rainAudioSource.Stop();
            Destroy(rainAudioSource.gameObject);
            rainAudioSource = null;
            Debug.Log("Rain sound stopped");
        }
    }

    // public void PlayWindSound(Vector3 position)
    // {
    //     if (windAudioSource == null && audioSourcePrefab != null && windClip != null)
    //     {
    //         // position.y += 10f; // Raise above ground, optional
    //         windAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
    //         windAudioSource.clip = windClip;
    //         windAudioSource.loop = true;
    //         windAudioSource.Play();
    //         Debug.Log("🌬️ Wind sound playing");
    //     }
    // }
    
    
    [SerializeField, Range(0f, 1f)] private float currentWindVolume = 1f; // For Inspector viewing only
    public void SetWindVolume(float volume)
    {
        if (windAudioSource != null)
        {
            windAudioSource.volume = volume;
            currentWindVolume = volume; // Update for Inspector
        }
    }
    
    public void PlayWindSound()
    {
        if (windAudioSource == null && windClip != null)
        {
            // Create a new AudioSource on the SoundManager (or use an existing one)
            windAudioSource = gameObject.AddComponent<AudioSource>();
            windAudioSource.clip = windClip;
            windAudioSource.loop = true;
            windAudioSource.spatialBlend = 0f; // 2D sound
            windAudioSource.Play();
            Debug.Log("Rain sound playing as 2D");
        }
    }
    

    public void PLayCatFoodSound(Vector3 position)
    {
        PlaySoundAtPosition(catfoodClip, position);
    }
}