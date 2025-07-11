using System;
using UnityEngine;

public class RoomCheck3 : MonoBehaviour
{
    public RoomCheckManager manager;

    private void Start()
    {
        manager = GetComponentInParent<RoomCheckManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        manager.bathroomChecked = true;
    }
}
