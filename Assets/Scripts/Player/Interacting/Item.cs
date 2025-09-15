using UnityEngine;

public class Item : DialogueBase, IPickupable
{
    public ItemScriptable itemScriptable;
    public GameObject itemVisuals;
    public float dropForce = 1f;
    public bool hasBeenDropped;
    
    public void Pickup(Transform handTransform)
    {
        if(InventoryManager.Current.IsInventoryFull())return;
        transform.SetParent(handTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
        PickupItem(itemScriptable);
        hasBeenDropped = false;
    }

    public void Drop(Transform handTransform)
    {
        InventoryManager.Current.RemoveItem();

        // Detach from hand
        transform.SetParent(null);

        // Reactivate physics
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Avoid tunneling
        col.isTrigger = false;

        // Calculate safe drop position
        Vector3 dropPos = handTransform.position + handTransform.forward * 1f;
        if (Physics.Raycast(handTransform.position, handTransform.forward, out RaycastHit hit, 1f))
        {
            dropPos = hit.point + Vector3.up * 0.2f;
        }
        else
        {
            dropPos += Vector3.up * 0.5f; // default offset
        }

        transform.position = dropPos;

        // Stop any existing motion
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Apply drop force
        rb.AddForce(handTransform.forward * dropForce, ForceMode.Impulse);

        // Reduce rolling
        rb.angularDamping = 5f;
        
        hasBeenDropped = true;
    }


    public void PickupItem(ItemScriptable thisItemsScriptableObject)
    {
        InventoryManager.Current.AddItem(thisItemsScriptableObject, gameObject);
    }
    
}
