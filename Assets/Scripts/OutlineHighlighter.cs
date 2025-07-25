using UnityEngine;

[RequireComponent(typeof(Outline))] // Ensure the component exists
public class OutlineHighlighter : MonoBehaviour
{
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            //outline.OutlineColor = Color.yellow;
            outline.enabled = false; // start disabled
        }
        else
        {
            Debug.LogWarning($"{name} is missing Outline component!");
        }
    }
    
    public void ShowOutline(bool show)
    {
        if (outline != null)
            outline.enabled = show;
    }
}