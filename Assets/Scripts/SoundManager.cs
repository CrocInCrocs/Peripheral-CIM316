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

public enum SoundCategory
{
    Master,
    Music,
    SFX
}

[System.Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioClip[] clips;
    public SoundCategory category = SoundCategory.SFX; // default SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source Prefab")] [SerializeField]
    private AudioSource audioSourcePrefab;

    [Header("Sound Library")] [SerializeField]
    private List<SoundEntry> soundEntries = new List<SoundEntry>();

    [Header("Audio Options")] [SerializeField]
    private AudioSource[] music; // assign in inspector

    [SerializeField] private AudioSource[] effects; // assign in inspector

    private Dictionary<SoundType, AudioClip[]> soundLibrary = new Dictionary<SoundType, AudioClip[]>();
    private Dictionary<SoundType, AudioSource> activeLoops = new Dictionary<SoundType, AudioSource>();
    private Dictionary<SoundType, SoundCategory> soundCategories = new Dictionary<SoundType, SoundCategory>();

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

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

            if (!soundCategories.ContainsKey(entry.type))
                soundCategories.Add(entry.type, entry.category);
        }

        LoadAudioSettings();
    }

    #region Play Methods

    public void PlaySound(SoundType type, Vector3 position, float volume = 1f)
    {
        if (!soundLibrary.ContainsKey(type) || audioSourcePrefab == null) return;

        AudioClip[] clips = soundLibrary[type];
        if (clips.Length == 0) return;

        AudioClip chosenClip = clips.Length == 1 ? clips[0] : clips[Random.Range(0, clips.Length)];
        AudioSource source = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        source.clip = chosenClip;
        source.volume = ApplyVolume(type, volume);
        source.Play();

        Destroy(source.gameObject, chosenClip.length);
    }

    public void PlayGlobalSound(SoundType type, float volume = 1f)
    {
        if (!soundLibrary.ContainsKey(type)) return;

        AudioClip[] clips = soundLibrary[type];
        if (clips.Length == 0) return;

        AudioClip chosenClip = clips.Length == 1 ? clips[0] : clips[Random.Range(0, clips.Length)];
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = chosenClip;
        source.volume = ApplyVolume(type, volume);
        source.spatialBlend = 0f; // 2D sound
        source.Play();

        Destroy(source, chosenClip.length);
    }

    public void StartLoop(SoundType type, Vector3 position, float volume = 1f)
    {
        if (activeLoops.ContainsKey(type) || !soundLibrary.ContainsKey(type) || audioSourcePrefab == null) return;

        AudioClip[] clips = soundLibrary[type];
        if (clips.Length == 0) return;

        AudioClip chosenClip = clips[0];
        AudioSource loopSource = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        loopSource.clip = chosenClip;
        loopSource.volume = ApplyVolume(type, volume);
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

    #endregion

    #region Volume Settings

    private float ApplyVolume(SoundType type, float baseVolume)
    {
        SoundCategory cat = SoundCategory.SFX;
        if (soundCategories.TryGetValue(type, out var c)) cat = c;

        float categoryVol = 1f;
        switch (cat)
        {
            case SoundCategory.Music: categoryVol = musicVolume; break;
            case SoundCategory.SFX: categoryVol = sfxVolume; break;
        }

        return baseVolume * categoryVol * masterVolume;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        AudioListener.volume = masterVolume; // optional: control AudioListener
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        UpdateLoopVolumes();
        UpdateAssignedAudioVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        UpdateLoopVolumes();
        UpdateAssignedAudioVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        UpdateLoopVolumes();
        UpdateAssignedAudioVolumes();
    }
    
    

    private void UpdateLoopVolumes()
    {
        foreach (var kvp in activeLoops)
        {
            if (kvp.Value != null)
                kvp.Value.volume = ApplyVolume(kvp.Key, 1f);
        }
    }

    private void UpdateAssignedAudioVolumes()
    {
        if (music != null)
        {
            foreach (var m in music)
                if (m != null)
                    m.volume = musicVolume * masterVolume;
        }

        if (effects != null)
        {
            foreach (var s in effects)
                if (s != null)
                    s.volume = sfxVolume * masterVolume;
        }
    }

    private void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        UpdateAssignedAudioVolumes();
    }

    public void SetWindVolume(float volume)
    {
        if (activeLoops.ContainsKey(SoundType.Wind))
        {
            AudioSource windSource = activeLoops[SoundType.Wind];
            if (windSource != null)
                windSource.volume = volume * sfxVolume * masterVolume;
        }
    }

    #endregion
}