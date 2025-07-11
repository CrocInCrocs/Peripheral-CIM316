using System;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public RoomCheckManager manager;

    private void Start()
    {
        manager = GetComponentInParent<RoomCheckManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        manager.diningChecked = true;
    }
}
