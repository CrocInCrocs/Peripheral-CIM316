using UnityEngine;


public static class TaskEvents
{
    public delegate void ChoreCompletedHandler(string taskName);
    public static event ChoreCompletedHandler OnChoreCompleted;

    public static event System.Action OnAllChoresCompleted;

    public static void InvokeChoreCompleted(string taskName)
    {
        OnChoreCompleted?.Invoke(taskName);
        Debug.Log("📢 ChoreCompleted event invoked for: " + taskName);
    }

    public static void InvokeAllChoresCompleted()
    {
        OnAllChoresCompleted?.Invoke();
        Debug.Log("✅ All chores completed event fired.");
    }
}