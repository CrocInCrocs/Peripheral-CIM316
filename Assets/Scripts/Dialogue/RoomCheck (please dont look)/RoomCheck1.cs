using System;
using UnityEngine;

public class RoomCheck1 : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        RoomCheckManager.Current.kitchenChecked = true;
    }
}
