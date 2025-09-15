using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.Rendering;

public class UIPopUpVanish : PopUpBase
{
    public void Start()
    {
        player = PeripheralGameManager.Current.returnFPController();
    }

    public void Update()
    {
        if (player == null && PeripheralGameManager.Current != null)
        {
            player = PeripheralGameManager.Current.returnFPController();
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
