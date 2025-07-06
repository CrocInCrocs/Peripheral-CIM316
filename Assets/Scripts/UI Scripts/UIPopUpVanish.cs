using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.Rendering;

public class UIPopUpVanish : PopUpBase
{
    public void Start()
    {
        player = PeripheralGameManager.Instance.returnFPController();
    }

    public void Update()
    {
        if (player == null)
        {
            player = PeripheralGameManager.Instance.returnFPController();
        }

        GameObject interactable = player.ReturnInteractableFromRayCast();
      
        if (interactable == parentGO)
        {
            popUpImage.SetActive(true);
        }
        else
        {
            popUpImage.SetActive(false);
        }
    }
}
