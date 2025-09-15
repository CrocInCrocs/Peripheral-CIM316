using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    [SerializeField] private GameObject choreListUI;
    [SerializeField] private string choreListItemName = "Paper";
    [SerializeField] private TextMeshProUGUI choreListText;

    [SerializeField] private List<string> choreSequence = new List<string> { "Take out the rubbish", "Wash Dishes", "Feed Cat" };
    private HashSet<string> completedChores = new HashSet<string>();

    public Color greenColour = Color.white;
    public Color redColour = Color.red;

    private int choresRevealed = 1;

    private void Awake()
    {
        if (choreListUI == null)
            choreListUI = GameObject.Find("TaskText_UI");

        if (choreListText == null)
        {
            GameObject textGO = GameObject.Find("TaskText_UI");
            if (textGO != null)
                choreListText = textGO.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        if (choreSequence.Count == 0 || choreListText == null)
        {
            Debug.LogWarning("Missing chore list or text component.");
            return;
        }

        UpdateChoreListText();
    }

    private void Update()
    {
        GameObject itemGO = InventoryManager.Current != null
            ? InventoryManager.Current.ReturnSelectedItemInInventory()
            : null;

        if (itemGO != null)
        {
            Item item = itemGO.GetComponent<Item>();
            if (item != null && item.itemScriptable != null &&
                item.itemScriptable.name == choreListItemName)
            {
                choreListUI?.SetActive(true);
                return;
            }
        }

        choreListUI?.SetActive(false);
    }

    public void OnChoreCompleted(string choreName)
    {
        choreName = choreName.Trim();

        if (completedChores.Contains(choreName))
            return;

        completedChores.Add(choreName);

        // Reveal the next chore only if we haven’t reached the end
        if (choresRevealed < choreSequence.Count)
        {
            choresRevealed++;
        }

        UpdateChoreListText();
    }

    private void UpdateChoreListText()
    {
        if (choreListText == null) return;

        string displayText = "";

        for (int i = 0; i < choresRevealed; i++)
        {
            string chore = choreSequence[i].Trim();

            if (completedChores.Contains(chore))
            {
                displayText += $"<color=#{ColorUtility.ToHtmlStringRGB(redColour)}><s>{chore}</s></color>\n";
            }
            else
            {
                displayText += $"<color=#{ColorUtility.ToHtmlStringRGB(greenColour)}>{chore}</color>\n";
            }
        }

        choreListText.text = displayText;
    }

    public int GetChoreCount() => choreSequence.Count;
    public int GetCompletedChoreCount() => completedChores.Count;
}