using System;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject lights;
    public bool lightsOff;
    private float timer;

    public void LightsOff()
    {
        lightsOff = true;
        
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            lights.SetActive(false);
        }
    }
}
