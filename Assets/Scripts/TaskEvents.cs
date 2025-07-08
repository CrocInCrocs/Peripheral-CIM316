using UnityEngine;


public static class TaskEvents
{
    public delegate void ChoreCompletedHandler(string taskName);

    public static event ChoreCompletedHandler OnChoreCompleted;

    public static void InvokeChoreCompleted(string taskName)
    {
        OnChoreCompleted?.Invoke(taskName);
        Debug.Log("📢 ChoreCompleted event invoked for: " + taskName);
    }
}