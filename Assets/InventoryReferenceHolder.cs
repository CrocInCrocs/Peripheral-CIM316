using System;
using UnityEngine;

public class InventoryReferenceHolder : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    private void Start()
    {
        InventoryManager.Current.UpdateReferences(inventorySlots);
    }
}
