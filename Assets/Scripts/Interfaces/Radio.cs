using UnityEngine;

public class Radio : ChoreBase
{
    [Header("Radio Settings")]
    [SerializeField] private SoundType staticSound = SoundType.Static;
    [SerializeField] private SoundType rainSound = SoundType.RainMusic;
    [SerializeField] private Transform radioTransform;

    private bool isOn = false;
    public float MusicVol = 0.1f;
    public float StaticVol = 10f;

    private bool IsRaining => PeripheralGameManager.Current != null && PeripheralGameManager.Current.isRaining;

    private void Start()
    {
        if (radioTransform == null)
            radioTransform = transform;
    }

    public override void CompleteChore()
    {
        base.CompleteChore();

        if (SoundManager.Instance == null) return;

        if (!isOn)
        {
            // Turn radio on: stop any other loops just in case
            SoundManager.Instance.StopLoop(staticSound);
            SoundManager.Instance.StopLoop(rainSound);

            if (IsRaining)
                SoundManager.Instance.StartLoop(rainSound, radioTransform.position, MusicVol);
            else
                SoundManager.Instance.StartLoop(staticSound, radioTransform.position, StaticVol);

            isOn = true;
        }
        else
        {
            // Turn radio off: stop both sounds to ensure nothing plays
            SoundManager.Instance.StopLoop(staticSound);
            SoundManager.Instance.StopLoop(rainSound);

            isOn = false;
        }
    }
}