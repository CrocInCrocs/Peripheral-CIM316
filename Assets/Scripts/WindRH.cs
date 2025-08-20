using UnityEngine;

public class WindRH : MonoBehaviour
{
    [Header("Wind Sound Managers")]
    public WindSoundManager[] windManagers;

    private void Awake()
    {
        // Optional: automatically find all WindSoundManagers in the scene
        if (windManagers == null || windManagers.Length == 0)
        {
            windManagers = FindObjectsOfType<WindSoundManager>();
        }
    }

    /// <summary>
    /// Apply a global indoor/outdoor volume to all wind managers
    /// </summary>
    public void SetAllWindVolumes(float indoorVolume, float outdoorVolume)
    {
        foreach (var wind in windManagers)
        {
            if (wind != null)
            {
                wind.SetVolumes(indoorVolume, outdoorVolume);
            }
        }
    }
}