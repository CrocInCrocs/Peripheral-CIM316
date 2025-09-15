using UnityEngine;

public class DishChore : ChoreBase
{
    [SerializeField] private float baseWashTime = 5f;

    private void Start()
    {
        DishTracker.Instance.AddDish(gameObject);
    }

    public override void StartChore()
    {
        timeToComplete = Sink.IsSinkOn ? baseWashTime * 0.5f : baseWashTime;
        Debug.Log("Dish chore started. Time to complete: " + timeToComplete + "s");
        base.StartChore();
    }

    public override void CompleteChore()
    {
        base.CompleteChore();

        // Notify the DishTracker this dish is done
        DishTracker.Instance.NotifyDishDone(gameObject);

        Debug.Log("Dish chore completed.");
        Destroy(gameObject); // Destroy dish object on completion
    }
}