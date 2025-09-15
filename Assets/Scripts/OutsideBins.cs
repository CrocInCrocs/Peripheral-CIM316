using DG.Tweening;
using UnityEngine;

public class OutsideBins : MonoBehaviour
{
    [SerializeField] private Transform bagDropPoint; // Position inside bin for bag to settle
    [SerializeField] private Oven binScript;         // Your bin open/close script
    [SerializeField] private SphereCollider binTriggerCollider;

    private bool lastIsOpenState;

    private void Start()
    {
        if (binScript == null)
            binScript = GetComponentInChildren<Oven>();

        if (binScript == null)
            Debug.LogWarning("Bin script not found on bin object!");

        binTriggerCollider = GetComponentInChildren<SphereCollider>();
        if (binTriggerCollider == null)
            Debug.LogWarning("SphereCollider trigger not found!");

        // Set collider initially according to bin open state
        lastIsOpenState = binScript != null && binScript.isOpen;
        if (binTriggerCollider != null)
            binTriggerCollider.enabled = lastIsOpenState;
    }

    private void Update()
    {
        if (binScript == null || binTriggerCollider == null) return;

        // Check if bin open state changed since last frame
        if (binScript.isOpen != lastIsOpenState)
        {
            binTriggerCollider.enabled = binScript.isOpen;
            lastIsOpenState = binScript.isOpen;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!binScript.isOpen) return;

        Item item = other.GetComponent<Item>();
        if (item == null || !item.hasBeenDropped) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.isKinematic = true;

        Sequence seq = DOTween.Sequence();

        seq.Join(other.transform.DOMove(bagDropPoint.position, 2f).SetEase(Ease.InOutSine));
        seq.Join(other.transform.DORotate(Vector3.zero, 2f).SetEase(Ease.InOutSine));

        seq.OnComplete(() =>
        {
            rb.isKinematic = true;
            // Fire the event
            TaskEvents.InvokeChoreCompleted("Take out the rubbish"); // or "Vacuum", "Dishes", etc.
            Debug.Log("Item dropped into bin.");
            
            //
            // // Remove item from inventory
            // InventoryManager.Current.RemoveItem();

      
        });
    }

}