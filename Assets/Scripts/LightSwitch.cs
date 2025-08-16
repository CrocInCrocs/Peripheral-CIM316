using System;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public PowerManager powerManager;
    private bool switchCase;
    public int BreakerIndex;
    private int PowerChangeValue = 2;
    private int CurrentPower = -1;

    public bool startOn;

    private void Start()
    {
        UpdateSwitch();
        if (startOn)
        {
            FlipSwitch();
        }
    }

    public void Interact()
    {
        FlipSwitch();
        Debug.Log("I am switching up");
    }

    private void FlipSwitch()
    {
        switchCase = !switchCase;
        if (switchCase)
        {
            Debug.Log("I am turning onnn");
            CurrentPower += PowerChangeValue;
        }
        else
        {
            Debug.Log("I am turning offf");
            CurrentPower -= PowerChangeValue;
        }
        
        UpdateSwitch();
    }

    public void UpdateSwitch()
    {
        powerManager.UpdateBreaker(BreakerIndex,CurrentPower);
    }
}
