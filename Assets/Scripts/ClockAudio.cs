using UnityEngine;

public class ClockAudio : MonoBehaviour
{
    private bool isPlayingTick;
    

    private bool CanPlay => PeripheralGameManager.Current != null && PeripheralGameManager.Current.clockCanPlay;
    
    

    public void StartTick(Vector3 pos)
    {
  
        if (!isPlayingTick && CanPlay)
        {
            SoundManager.Instance.StartLoop(SoundType.ClockTick, pos, 1f);
            isPlayingTick = true;
        }
    }
    
    

    public void StopTick()
    {
        if (isPlayingTick)
        {
            SoundManager.Instance.StopLoop(SoundType.ClockTick);
            isPlayingTick = false;
        }
    }
}