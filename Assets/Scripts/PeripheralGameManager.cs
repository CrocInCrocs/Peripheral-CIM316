using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PeripheralGameManager : MonoBehaviour
{
    public static PeripheralGameManager Instance;

    [Header("Chore Tracking")]
    [SerializeField] public int totalChores = 10;
    [SerializeField] private float choresCompleted = 0;
    [SerializeField] private TextMeshProUGUI choreText;
    [SerializeField] private TaskController taskController; // assign in inspector
    public GameObject rain;
    public FPController _player;
    public FadeController fade;
    
    private HashSet<string> completedChores = new HashSet<string>();
    
    // List of chores to track — adjust as needed
    private List<string> trackedChores = new List<string> { "Take out the rubbish", "Wash Dishes", "Feed Cat" };
    
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
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
        if (completedChores.Contains(taskName))
            return; // Already completed this chore, ignore

        if (!trackedChores.Contains(taskName))
            return; // Not a chore we track, ignore

        completedChores.Add(taskName);
        choresCompleted = completedChores.Count;

        Debug.Log($"✅ Task completed: {taskName}");
        choreText.text = $"Chores: {choresCompleted}/{totalChores}";

        // Tell TaskController to update visuals
        taskController?.OnChoreCompleted(taskName);

        // Fire global event for this chore completed
        TaskEvents.InvokeChoreCompleted(taskName);

        // Check if all tracked chores are complete
        if (choresCompleted >= totalChores)
        {
            Debug.Log("✅ All chores complete! Go to sleep.");
            TaskEvents.InvokeAllChoresCompleted();
        }
    }
    
    

    public void RainStart()
    {
        rain.SetActive(true);
    }
    
    public void StartSleep()
    {
        fade.StartFadeIn();
    }

    public void StartWakeUp()
    {
        fade.StartFadeOut();
    }
}