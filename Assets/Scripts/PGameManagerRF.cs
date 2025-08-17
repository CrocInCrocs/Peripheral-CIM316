using UnityEngine;

public class PGameManagerRF : MonoBehaviour
{

    public TaskController taskController;
    public GameObject rain;
    public FPController playerController;
    public FadeController fadeController;

    public GameObject roomCheck1;
    public GameObject roomCheck2;
    public GameObject roomCheck3;
    public GameObject roomCheck4;
    public GameObject roomCheck5;

    public GameObject clockTrigger; // NEW ✅
    public LightController lightController;
    public ClockAudio clockAudio;   // still needed for playing audio

    private void Start()
    {
        if (PeripheralGameManager.Current != null)
        {
            PeripheralGameManager.Current.UpdateReferences(
                taskController,
                rain,
                playerController,
                fadeController,
                new GameObject[] { roomCheck1, roomCheck2, roomCheck3, roomCheck4, roomCheck5 },
                lightController,
                clockTrigger,  
                clockAudio    
            );
        }
    }
}