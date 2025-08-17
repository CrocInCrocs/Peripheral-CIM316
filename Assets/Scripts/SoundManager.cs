using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    DoorOpen,
    DoorClose,
    Footstep,
    CurtainOpen,
    CurtainClose,
    Printer,
    CatFood,
    ComputerOn,
    ComputerOff,
    CCTVView,
    SwitchCamera,
    SinkOn,
    SinkOff,
    Wind,
    RainOn,
    RainOff,
    Thunder,
    SlidingDoor,
    RainMusic,
    Static,
    Toilet,
    ClockTick,
    CardboardOpen,
    CardboardClose
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source Prefab (Required)")] [SerializeField]
    private AudioSource audioSourcePrefab;

    [Header("Sound Library")] [SerializeField]
    private List<SoundEntry> soundEntries = new List<SoundEntry>();

    private Dictionary<SoundType, AudioClip[]> soundLibrary = new Dictionary<SoundType, AudioClip[]>();
    private Dictionary<SoundType, AudioSource> activeLoops = new Dictionary<SoundType, AudioSource>();

    [System.Serializable]
    public class SoundEntry
    {
        public SoundType type;
        public AudioClip[] clips; 
    }
    
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        foreach (var entry in soundEntries)
        {
            if (!soundLibrary.ContainsKey(entry.type))
                soundLibrary.Add(entry.type, entry.clips);
        }
    }


    public void PlaySound(SoundType type, Vector3 position, float volume = 1f)
    {
        if (!soundLibrary.ContainsKey(type) || audioSourcePrefab == null) return;

        AudioClip[] clips = soundLibrary[type];
        if (clips == null || clips.Length == 0) return;

        AudioClip chosenClip = clips.Length == 1 ? clips[0] : clips[Random.Range(0, clips.Length)];

        AudioSource source = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        source.clip = chosenClip;
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, chosenClip.length);
    }


    public void PlayGlobalSound(SoundType type, float volume = 1f)
    {
        if (!soundLibrary.ContainsKey(type)) return;

        AudioClip[] clips = soundLibrary[type];
        if (clips == null || clips.Length == 0) return;

        AudioClip chosenClip = clips.Length == 1 ? clips[0] : clips[Random.Range(0, clips.Length)];

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = chosenClip;
        source.volume = volume;
        source.spatialBlend = 0f; // 2D sound
        source.Play();

        Destroy(source, chosenClip.length);
    }


    public void StartLoop(SoundType type, Vector3 position, float volume = 1f)
    {
        if (activeLoops.ContainsKey(type) || !soundLibrary.ContainsKey(type) || audioSourcePrefab == null)
            return;

        AudioClip[] clips = soundLibrary[type];
        if (clips == null || clips.Length == 0) return;

        AudioClip chosenClip = clips[0]; 
        AudioSource loopSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        loopSource.clip = chosenClip;
        loopSource.volume = volume;
        loopSource.loop = true;
        loopSource.Play();

        activeLoops[type] = loopSource;
    }

    public void StopLoop(SoundType type)
    {
        if (!activeLoops.ContainsKey(type)) return;

        AudioSource loopSource = activeLoops[type];
        loopSource.Stop();
        Destroy(loopSource.gameObject);
        activeLoops.Remove(type);
    }

    public void SetWindVolume(float volume)
    {
        if (activeLoops.ContainsKey(SoundType.Wind))
        {
            AudioSource windSource = activeLoops[SoundType.Wind];

            // Check if it still exists
            if (windSource != null)
            {
                windSource.volume = Mathf.Clamp01(volume);
            }
            else
            {
                // Re-create the wind loop if missing
                activeLoops.Remove(SoundType.Wind);
                StartLoop(SoundType.Wind, Vector3.zero, Mathf.Clamp01(volume)); 
            }
        }
    }
}