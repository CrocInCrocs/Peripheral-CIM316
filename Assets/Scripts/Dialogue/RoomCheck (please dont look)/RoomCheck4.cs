using System;
using UnityEngine;

public class RoomCheck4 : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        RoomCheckManager.Current.garageChecked = true;
    }
}
