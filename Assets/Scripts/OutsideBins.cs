using DG.Tweening;
using UnityEngine;

public class OutsideBins : MonoBehaviour
{
    [SerializeField] private Transform bagDropPoint;        // Position inside bin for bag to settle
    [SerializeField] private Oven binScript;                // Your bin open/close script
    [SerializeField] private Collider[] binTriggerColliders; // Assign any colliders here (Sphere, Box, Capsule, etc.)

    private bool lastIsOpenState;

    private void Start()
    {
        if (binScript == null)
            binScript = GetComponentInChildren<Oven>();

        if (binScript == null)
            Debug.LogWarning("Bin script not found on bin object!");

        if (binTriggerColliders == null || binTriggerColliders.Length == 0)
            Debug.LogWarning("No bin trigger colliders assigned!");

        // Initially enable/disable all colliders based on bin state
        lastIsOpenState = binScript != null && binScript.isOpen;
        SetCollidersEnabled(lastIsOpenState);
    }

    private void Update()
    {
        if (binScript == null || binTriggerColliders == null || binTriggerColliders.Length == 0)
            return;

        if (binScript.isOpen != lastIsOpenState)
        {
            lastIsOpenState = binScript.isOpen;
            SetCollidersEnabled(lastIsOpenState);
        }
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (var col in binTriggerColliders)
        {
            if (col != null)
                col.enabled = enabled;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (binScript == null || !binScript.isOpen) return;

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
            TaskEvents.InvokeChoreCompleted("Take out the rubbish");
            Debug.Log("Item dropped into bin.");
            // Inventory removal logic here if needed
        });
    }
}