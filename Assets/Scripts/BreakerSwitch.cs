using System;
using UnityEngine;

public class BreakerSwitch : MonoBehaviour, IInteractable
{
    public Material onMaterial;
    public Material offMaterial;
    public bool switchState;
    public PowerManager powerManager;
    public int breakerIndex;
    public int powerChangeValue;
    public void Interact()
    {
        UpdateSwitch();
    }

    private void Start()
    {
        switchState = false;
        UpdateSwitch();
    }

    public void UpdateSwitch()
    {
        switchState = !switchState;
        if (switchState)
        {
            powerManager.ChangeBreakerState(breakerIndex, true);
            gameObject.GetComponent<MeshRenderer>().material = onMaterial;
        }
        else
        {
            powerManager.ChangeBreakerState(breakerIndex, false);
            gameObject.GetComponent<MeshRenderer>().material = offMaterial;
        }
    }
}
