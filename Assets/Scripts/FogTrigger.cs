using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = Color.gray;
    public FogMode fogMode = FogMode.Exponential;
    public float fogDensity = 0.01f;

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
            RenderSettings.fog = false;
        }
    }
}