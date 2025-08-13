using System;
using UnityEngine;

public class LightPowerManager : MonoBehaviour
{
    public Light lightSource;
    public bool IsLighton;
    public float savedPower;
    public bool HasPower = true;

    private void Awake()
    {
        savedPower = lightSource.intensity;
        HasPower = true;
    }

    public void TurnoffLight()
    {
        IsLighton = false;
        UpdateLight();
    }
    public void TurnonLight()
    {
        IsLighton = true;
        UpdateLight();
    }

    public void UpdateLight()
    {
        if (HasPower)
        {
            if (IsLighton)
            {
                lightSource.intensity = savedPower;
            }
            else
            {
                lightSource.intensity = 0;
            }
        }
        else
        {
            lightSource.intensity = 0;
        }
    }
}
