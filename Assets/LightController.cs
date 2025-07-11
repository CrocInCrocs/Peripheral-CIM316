using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject lights;

    public void LightsOff()
    {
        lights.SetActive(false);
    }
}
