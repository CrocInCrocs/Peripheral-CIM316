using System;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        RoomCheckManager.Current.diningChecked = true;
    }
}
