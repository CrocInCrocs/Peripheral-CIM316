using TMPro;
using UnityEngine;

public class PeripheralGameManager : MonoBehaviour
{

    
    private static PeripheralGameManager _current;
    public static PeripheralGameManager Current { get { return _current; } }

   
    [SerializeField] private TaskController taskController; // Assign in inspector

    [SerializeField] public bool allChoresDone = false; // For inspector view, read-only
    
    
    public GameObject rain;
    public FPController _player;
    public FadeController fade;

    public GameObject roomCheck1, roomCheck2, roomCheck3, roomCheck4, roomCheck5;
    public LightController LightController;
    private bool mainGameRunning = true;
    // NEW: track rain state
    [Header("Debug")]
    [SerializeField] private bool debugIsRaining = false;
    public bool isRaining { get; private set; } = false;

    
    public bool clockCanPlay { get; private set; } = false;


    public void EnableClock()
    {
        clockCanPlay = true;
   
    }
    
    
    
    
    
    private void UpdateDebug()
    {
        debugIsRaining = isRaining;
    }
    
    
    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _current = this;
            DontDestroyOnLoad(gameObject);
        }

        if (SoundManager.Instance != null)
        {
            // Start looping wind sound
            SoundManager.Instance.StartLoop(SoundType.Wind, transform.position);
        }
    }
    
    
    
    
    
    private void OnEnable()
    {
        TaskEvents.OnChoreCompleted += HandleChoreComplete;
    }

    private void OnDisable()
    {
        TaskEvents.OnChoreCompleted -= HandleChoreComplete;
    }
    
    public FPController returnFPController()
    {
        return _player;
    }

    public void SetFPController(FPController player)
    {
        _player = player;
        
    }

    private void HandleChoreComplete(string taskName)
    {
        taskName = taskName.Trim();

        Debug.Log($"✅ Task completed: {taskName}");

        // Tell TaskController to update its UI state
        taskController?.OnChoreCompleted(taskName);

        // Update UI count based on TaskController's completed chores count
        int completedCount = taskController != null ? taskController.GetCompletedChoreCount() : 0;
        int totalChores = taskController != null ? taskController.GetChoreCount() : 0;

        // choreText.text = $"Chores: {completedCount}/{totalChores}";

        allChoresDone = (completedCount >= totalChores && totalChores > 0); 
        
        if (allChoresDone)
        {
            Debug.Log("🎉 All chores completed! GO TO SLEEP");
            // StartSleep(); // Act on the flag being true
        }
    }

    public void RainStart()
    {
        rain.SetActive(true);
        isRaining = true; // <-- set raining flag
    }

    public void RainStop()
    {
        rain.SetActive(false);
        isRaining = false; // <-- clear raining flag
    }


    public void StartSleep()
    {
        fade.StartFadeIn();
        // _player.DisableInput();
    }

    public void StartWakeUp()
    {
        fade.StartFadeOut();
        _player.EnableInput();
    }

    public void EnableTriggerBoxes()
    {
        roomCheck1.SetActive(true);
        roomCheck2.SetActive(true);
        roomCheck3.SetActive(true);
        roomCheck4.SetActive(true);
        roomCheck5.SetActive(true);
    }

    public void LightsOut()
    {
        LightController.LightsOff();
    }

    public void SetGameRunningState(bool state)
    {
        mainGameRunning = state;
    }
    public bool IsGameRunning()
    {
        return mainGameRunning;
    }
}