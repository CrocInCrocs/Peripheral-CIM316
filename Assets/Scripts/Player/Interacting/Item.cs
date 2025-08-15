using UnityEngine;
using UnityEngine.Serialization;

public class Item : DialogueBase, IPickupable
{
    public ItemScriptable itemScriptable;
    public GameObject itemVisuals;
    public float dropForce = 1f;
    public bool hasBeenDropped;
    public Transform playerTransform; 
    [SerializeField] private float dropDistance = 0.5f;
    [FormerlySerializedAs("dropHorizontalOffset")] [SerializeField] private float DropPostion = -0.4f;
    public bool pickupPositionChanged; // Tracks if pickup position was changed
    
    [Header("Pickup Settings")]
    public bool canBePickedUp = true; // NEW: control whether the item can be picked up
    
    
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
        else
            Debug.LogWarning("Player object with tag 'Player' not found!");
    }
    
    public void Pickup(Transform handTransform)
    {
        
        if (!canBePickedUp) return; // Skip pickup if not allowed
        if(InventoryManager.Current.IsInventoryFull())return;
        transform.SetParent(handTransform);
        
        // Special offset for paper only
        if (itemScriptable != null && itemScriptable.nameOfItem == "Paper")
        {
            transform.localPosition = new Vector3(0.25f, .22f, 0.02f);
            transform.localRotation = Quaternion.Euler(13f, 35.6f, 2.7f);
            pickupPositionChanged = true;
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            pickupPositionChanged = false;
        }

        

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

        Vector3 forwardDir = playerTransform.forward.normalized;  // use player's forward direction

        Vector3 rightDir = playerTransform.right.normalized;
        Vector3 dropPos = handTransform.position + forwardDir * dropDistance + rightDir * DropPostion;

        if (Physics.Raycast(handTransform.position, forwardDir, out RaycastHit hit, 1f))
        {
            dropPos = hit.point + Vector3.up * 0.2f;
        }
        else
        {
            dropPos += Vector3.up * 0.5f;
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
