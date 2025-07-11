using System;
using UnityEngine;

public class RoomCheck4 : MonoBehaviour
{
    public RoomCheckManager manager;

    private void Start()
    {
        manager = GetComponentInParent<RoomCheckManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        manager.garageChecked = true;
    }
}
