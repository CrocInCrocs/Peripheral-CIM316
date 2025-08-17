using TMPro;
using UnityEngine;

public class TaskControllerRF : MonoBehaviour
{
    [Header("UI References")]
    public GameObject choreListUI;
    public TextMeshProUGUI choreListText;

    [Header("Physical Paper References")]
    public TextMeshProUGUI takeOutRubbishText;
    public TextMeshProUGUI washDishesText;
    public TextMeshProUGUI feedCatText;

    private void Start()
    {
        if (TaskController.Current != null)
        {
            TaskController.Current.UpdateReferences(
                choreListUI,
                choreListText,
                takeOutRubbishText,
                washDishesText,
                feedCatText
            );
        }
    }
}