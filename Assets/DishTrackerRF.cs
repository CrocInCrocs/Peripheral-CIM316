using System.Collections.Generic;
using UnityEngine;

public class DishTrackerRF : MonoBehaviour
{
    [Header("Dish References")]
    public List<GameObject> dishes;

    private void Start()
    {
        if (DishTracker.Instance != null)
        {
            DishTracker.Instance.UpdateReferences(dishes);
        }
    }
}