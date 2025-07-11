using System;
using UnityEngine;

public class RoomCheck2 : MonoBehaviour
{
    public RoomCheckManager manager;

    private void Start()
    {
        manager = GetComponentInParent<RoomCheckManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        manager.loungeChecked = true;
    }
}
