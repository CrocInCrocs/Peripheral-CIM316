using UnityEngine;

public class Sink : ChoreBase
{
    [Header("Visuals")]
    [SerializeField] private GameObject sinkWaterToggle;  // Visual water stream
    [SerializeField] private GameObject sinkBubbles;      // GameObject holding particle system

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string boolParameter = "SinkOn"; // replace onTrigger/offTrigger

    public static bool IsSinkOn { get; private set; } = false;

    public override void StartChore()
    {
        base.StartChore();
        
    }

    public override void CompleteChore()
    {
        base.CompleteChore();

        IsSinkOn = !IsSinkOn;

        if (sinkWaterToggle != null)
            sinkWaterToggle.SetActive(IsSinkOn);

        if (sinkBubbles != null)
            sinkBubbles.SetActive(IsSinkOn);

        Debug.Log("Sink is now " + (IsSinkOn ? "ON" : "OFF"));

        if (animator != null)
            animator.SetBool(boolParameter, IsSinkOn);
        
        // Play sound
        if (SoundManager.Instance != null)
        {
            if (IsSinkOn)
                SoundManager.Instance?.StartSinkLoopSound(transform.position);
            else
                SoundManager.Instance?.StopSinkLoopSound();
            
        }
        
        
        
    }
    // private void TriggerOnAnimation()
    // {
    //     if (animator != null && !string.IsNullOrEmpty(onTrigger))
    //     {
    //         Debug.Log("Triggering TurnOnSink animation");
    //         animator.SetTrigger(onTrigger);
    //     }
    // }
    //
    // private void TriggerOffAnimation()
    // {
    //     if (animator != null && !string.IsNullOrEmpty(offTrigger))
    //     {
    //         Debug.Log("Triggering TurnOffSink animation");
    //         animator.SetTrigger(offTrigger);
    //     }
    // }
}
