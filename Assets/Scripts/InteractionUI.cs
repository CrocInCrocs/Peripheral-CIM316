using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Prompt Messages")]
    // [SerializeField] private string pickUpPrompt = "E - Pick Up";
    [SerializeField] private string inspectPrompt = "F - Inspect";
    [SerializeField] private string dropPrompt = "Q - Drop";
    // [SerializeField] private string interactPrompt = "E - Interact";
    // [SerializeField] private string chorePrompt = "E - Start Chore";

    private void Awake()
    {
        Instance = this;
        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        promptText.text = message;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        promptText.text = "";
        promptText.gameObject.SetActive(false);
    }

    // Accessor methods for prompt values
    public string GetPickupInspectPrompt() => $"{inspectPrompt}";
    public string GetDropPrompt() => dropPrompt;
    // public string GetInteractPrompt() => interactPrompt;
    // public string GetChorePrompt() => chorePrompt;
}