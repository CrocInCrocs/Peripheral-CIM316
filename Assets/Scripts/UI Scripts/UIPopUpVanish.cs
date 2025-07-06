using System;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.Rendering;

public class UIPopUpManager : PopUpBase
{

    public void Update()
    {
        if (player.RayCastFromCamera())
        {
            popUpImage.SetActive(true);
        }
        else
        {
            popUpImage.SetActive(false);
        }
    }
}
