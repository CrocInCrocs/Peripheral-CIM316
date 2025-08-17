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


    private bool hasPlayed = false;

    public void EnableTrigger()
    {
        // Collider col = GetComponentInChildren<Collider>();
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
     
            Vector3 pos = transform.position;


            SoundManager.Instance.PlaySound(SoundType.ClockTick, transform.position, 1f);
            Debug.Log("clock played");
            hasPlayed = true;


            GetComponent<Collider>().enabled = false;
        }
    }
}