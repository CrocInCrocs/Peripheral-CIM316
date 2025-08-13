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
            {
                // Start the looping "SinkOn" sound
                SoundManager.Instance.StartLoop(SoundType.SinkOn, transform.position);
            }
            else
            {
                // Stop the looping "SinkOn" sound
                SoundManager.Instance.StopLoop(SoundType.SinkOn);

                // Optionally play a one-shot "SinkOff" sound
                SoundManager.Instance.PlaySound(SoundType.SinkOff, transform.position);
            }
        }
        
        
        
    }

}
