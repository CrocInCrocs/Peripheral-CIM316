using System.Collections.Generic;
using UnityEngine;

public class DishTracker : MonoBehaviour
{
    public static DishTracker Instance;

    [SerializeField] private List<GameObject> dishes = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddDish(GameObject dish)
    {
        if (!dishes.Contains(dish))
            dishes.Add(dish);
    }

    public void NotifyDishDone(GameObject dish)
    {
        if (dish != null && dishes.Contains(dish))
        {
            dishes.Remove(dish);
            Debug.Log($"✅ Dish cleaned. {dishes.Count} remaining.");

            if (dishes.Count == 0)
            {
                Debug.Log("🍽️ All dishes cleaned. Chore complete!");
                TaskEvents.InvokeChoreCompleted("Wash Dishes");
            }
        }
    }
}