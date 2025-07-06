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
    
    private void HandleChoreComplete(string taskName)
    {
        Debug.Log($"✅ Task completed: {taskName}");

        // Notify the TaskController manually
        taskController?.OnChoreCompleted(taskName);

        choresCompleted += 1f;
        choreText.text = $"Chores: {choresCompleted}/{totalChores}";

        if (choresCompleted >= totalChores)
            Debug.Log("✅ All chores complete! Game over.");
    }

    public void RainStart()
    {
        rain.SetActive(true);
    }
}