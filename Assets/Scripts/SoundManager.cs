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
    Toilet
    
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source Prefab (Required)")]
    [SerializeField] private AudioSource audioSourcePrefab;

    [Header("Sound Library")]
    [SerializeField] private List<SoundEntry> soundEntries = new List<SoundEntry>();

    private Dictionary<SoundType, AudioClip[]> soundLibrary = new Dictionary<SoundType, AudioClip[]>();
    private Dictionary<SoundType, AudioSource> activeLoops = new Dictionary<SoundType, AudioSource>();

    [System.Serializable]
    public class SoundEntry
    {
        public SoundType type;
        public AudioClip[] clips; // Can have one or multiple clips (random pick)
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

        // Build dictionary
        foreach (var entry in soundEntries)
        {
            if (!soundLibrary.ContainsKey(entry.type))
                soundLibrary.Add(entry.type, entry.clips);
        }
    }

    // ===== One-shot sound =====
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

    // ===== 2D global sound =====
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

    // ===== Looping sound =====
    public void StartLoop(SoundType type, Vector3 position, float volume = 1f)
    {
        if (activeLoops.ContainsKey(type) || !soundLibrary.ContainsKey(type) || audioSourcePrefab == null)
            return;

        AudioClip[] clips = soundLibrary[type];
        if (clips == null || clips.Length == 0) return;

        AudioClip chosenClip = clips[0]; // Only first clip for looping

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
            activeLoops[SoundType.Wind].volume = Mathf.Clamp01(volume);
        }
    }
    

}