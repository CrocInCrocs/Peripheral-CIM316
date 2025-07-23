using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))] // Ensure the component exists
public abstract class ChoreBase : MonoBehaviour, IChoreable, IInteractable
{
    public float timeToComplete = 3f;
    public float currentProgress = 0f;
    public bool isWorking = false;

    public event Action<float> OnChoreProgress;
    public event Action OnChoreStarted;
    public event Action OnChoreStopped;
    public event Action OnChoreCompleted;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();

        if (outline != null)
        {
            outline.OutlineColor = Color.yellow;  // force color

            // Disable outline to start OFF
            outline.enabled = false;
        }
        else
        {
            Debug.LogWarning($"{name} has no Outline component!");
        }
    }

    public void ShowOutline(bool show)
    {
        if (outline != null)
        {
            outline.enabled = show;
        }
    }

    public bool IsChoreActive()
    {
        return isWorking;
    }

    protected void Update()
    {
        if (isWorking)
        {
            currentProgress += Time.deltaTime;
            OnChoreProgress?.Invoke(currentProgress / timeToComplete);

            if (currentProgress >= timeToComplete)
            {
                CompleteChore();
            }
        }
    }

    public virtual void StartChore()
    {
        if (isWorking) return;

        isWorking = true;
        currentProgress = 0f;
        OnChoreStarted?.Invoke();
    }

    public virtual void StopChore()
    {
        isWorking = false;
        OnChoreStopped?.Invoke();
    }

    public virtual void CompleteChore()
    {
        isWorking = false;
        OnChoreCompleted?.Invoke();
    }

    public virtual void Interact()
    {
        StartChore();
    }
}