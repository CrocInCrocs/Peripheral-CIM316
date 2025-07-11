using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    [Header("Fog Settings - Inside Trigger")]
    public Color fogColor = Color.cyan;
    public FogMode fogMode = FogMode.Exponential;
    public float fogDensity = 0.01f;

    [Header("Fog Settings - Outside Trigger")]
    public Color outsideFogColor = Color.gray;
    public float outsideFogDensity = 0.02f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogMode = fogMode;
            RenderSettings.fogDensity = fogDensity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fog = true; // Keep fog on
            RenderSettings.fogColor = outsideFogColor;
            RenderSettings.fogDensity = outsideFogDensity;
            // Keep the same fogMode or change it if needed
        }
    }
}