using TMPro;
using UnityEngine;

public class PeripheralGameManager : MonoBehaviour
{
    [SerializeField] private TaskController taskController;
    [SerializeField] public bool allChoresDone = false;
    public GameObject rain;
    public FPController _player;
    public FadeController fade;
    public GameObject roomCheck1, roomCheck2, roomCheck3, roomCheck4, roomCheck5;
    public LightController LightController;
    private bool mainGameRunning = true;
    [SerializeField] private bool debugIsRaining = false;

    public bool isRaining { get; private set; } = false;
    public bool clockCanPlay { get; private set; } = false;
    private static PeripheralGameManager _current;

    public static PeripheralGameManager Current
    {
        get { return _current; }
    }

    // rory is a noob
    public void UpdateReferences(TaskController newTaskController,
        GameObject newRain,
        FPController newPlayer,
        FadeController newFade,
        GameObject[] roomChecks,
        LightController newLightController,
        GameObject newClockTrigger,
        ClockAudio newClockAudio)
    {
        taskController = newTaskController;
        rain = newRain;
        _player = newPlayer;
        fade = newFade;

        if (roomChecks != null && roomChecks.Length == 5)
        {
            roomCheck1 = roomChecks[0];
            roomCheck2 = roomChecks[1];
            roomCheck3 = roomChecks[2];
            roomCheck4 = roomChecks[3];
            roomCheck5 = roomChecks[4];
        }

        LightController = newLightController;
        clockTrigger = newClockTrigger;  
        clockAudio = newClockAudio;      
    }
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

        // Debug.Log($"✅ Task completed: {taskName}");


        taskController?.OnChoreCompleted(taskName);


        int completedCount = taskController != null ? taskController.GetCompletedChoreCount() : 0;
        int totalChores = taskController != null ? taskController.GetChoreCount() : 0;


        allChoresDone = (completedCount >= totalChores && totalChores > 0);

        if (allChoresDone)
        {
            // Debug.Log("🎉 All chores completed! GO TO SLEEP");
        }
    }

    public void RainStart()
    {
        rain.SetActive(true);
        isRaining = true;
    }

    public void RainStop()
    {
        rain.SetActive(false);
        isRaining = false;
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


    public ClockAudio clockAudio; 
    public GameObject clockTrigger; 
    public void EnableClockTrigger()
    {
        if (clockAudio != null)
        {
            clockAudio.EnableTrigger();
        }
    
        if (clockTrigger != null)
        {
            clockTrigger.SetActive(true);
        }
    }
}