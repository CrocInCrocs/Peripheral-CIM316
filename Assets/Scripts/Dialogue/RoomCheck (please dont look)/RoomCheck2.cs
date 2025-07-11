using System;
using UnityEngine;

public class RoomCheck2 : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        RoomCheckManager.Current.loungeChecked = true;
    }
}
