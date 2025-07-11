using System;
using UnityEngine;

public class RoomCheck3 : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        RoomCheckManager.Current.bathroomChecked = true;
    }
}
