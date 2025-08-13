using System;
using UnityEngine;

public class BreakerSwitch : MonoBehaviour, IInteractable
{
    public Material onMaterial;
    public Material offMaterial;
    public bool switchState;
    public GameObject tempCubevisual;
    public BreakerController breakerController;
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
