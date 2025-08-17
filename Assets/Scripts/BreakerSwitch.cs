using System;
using UnityEngine;

public class BreakerSwitch : MonoBehaviour, IInteractable
{
    public Material onMaterial;
    public Material offMaterial;
    public bool switchState;
    public GameObject tempCubevisual;
    public BreakerController breakerController;
    public bool enabled;
    public void Interact()
    {
        UpdateSwitch();
    }

    private void Start()
    {
        switchState = false;
        UpdateSwitch();
    }

    private void Update()
    {
        if (enabled)
        {
            breakerController.TurnOffBreaker();
            tempCubevisual.GetComponent<MeshRenderer>().material = offMaterial;
        }
    }

    public void UpdateSwitch()
    {
        if(enabled) return;
        switchState = !switchState;
        if (switchState)
        {
            breakerController.TurnOnBreaker();
            tempCubevisual.GetComponent<MeshRenderer>().material = onMaterial;
        }
        else
        {
            breakerController.TurnOffBreaker();
            tempCubevisual.GetComponent<MeshRenderer>().material = offMaterial;
        }
    }
}
