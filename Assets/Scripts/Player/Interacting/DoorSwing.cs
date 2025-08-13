using UnityEngine;
using DG.Tweening;

public class DoorSwing : MonoBehaviour, IInteractable
{
    public bool isOpen = false;
    public float swingAngle = 90f;
    public float swingDuration = 2f;
    public Vector3 hingeAxis = Vector3.up;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.AngleAxis(swingAngle, hingeAxis) * closedRotation;
    }

    public void Interact()
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        // Play open/close sound via SoundManager
        if (SoundManager.Instance != null)
        {
            SoundType type = isOpen ? SoundType.DoorOpen : SoundType.DoorClose;
            SoundManager.Instance.PlaySound(type, transform.position);
        }

        // Animate door swing
        transform.DOKill();
        transform.DORotateQuaternion(targetRotation, swingDuration)
            .SetEase(Ease.OutCubic);
    }
}