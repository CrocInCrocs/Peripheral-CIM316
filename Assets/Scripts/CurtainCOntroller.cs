using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CurtainCOntroller : ChoreBase
{
    [Header("Slide Axis")]
    public bool slideX = false;
    public bool slideY = false;
    public bool slideZ = true;

    [Header("Slide Settings")]
    public bool slideOppositeDirection = false;
    public float slideDistance = 1f;
    public float slideDuration = 2f;

    [Header("Auto Return Settings")]
    public bool enableAutoReturn = true;
    public float autoReturnTime = 120f;
    
    [Header("Scale Change Settings")]
    public bool enableScaleChange = false;
    public bool scaleX = false;
    public bool scaleY = false;
    public bool scaleZ = false;
    public float scaleAmount = 0.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Vector3 closedScale;
    private Vector3 openScale;
    [SerializeField] private bool isAtClosedPosition = true;
    private Coroutine autoReturnCoroutine;
    
    [Header("Sound Settings")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private bool playSoundAtWorldPosition = true;
    

    void Start()
    {
        closedPosition = transform.localPosition;
        closedScale = transform.localScale;

        Vector3 direction = Vector3.zero;

        if (slideX) direction = slideOppositeDirection ? -Vector3.right : Vector3.right;
        else if (slideY) direction = slideOppositeDirection ? -Vector3.up : Vector3.up;
        else if (slideZ) direction = slideOppositeDirection ? -Vector3.forward : Vector3.forward;
        
        openPosition = closedPosition + direction * slideDistance;
        // Set open scale
        openScale = closedScale;
        if (enableScaleChange)
        {
            if (scaleX) openScale.x += scaleAmount;
            if (scaleY) openScale.y += scaleAmount;
            if (scaleZ) openScale.z += scaleAmount;
        }
        

 
    }

    public override void Interact()
    {
        base.Interact();

        if (autoReturnCoroutine != null)
        {
            StopCoroutine(autoReturnCoroutine);
            autoReturnCoroutine = null;
        }
    }

    public override void CompleteChore()
    {
        base.CompleteChore();

        ToggleCurtain();

        // if (autoReturnCoroutine != null)
        //     StopCoroutine(autoReturnCoroutine);
        //
        // if (enableAutoReturn)
        //     autoReturnCoroutine = StartCoroutine(AutoReturnAfterDelay());
    }

    private void ToggleCurtain()
    {
        Vector3 targetPosition = isAtClosedPosition ? openPosition : closedPosition;
        Vector3 targetScale = isAtClosedPosition ? openScale : closedScale;

        transform.DOKill();
        transform.DOLocalMove(targetPosition, slideDuration)
            .SetEase(Ease.OutCubic);

        if (enableScaleChange)
        {
            transform.DOScale(targetScale, slideDuration)
                .SetEase(Ease.OutCubic);
        }
        
        // Play sound based on closing/opening
        // SoundManager.Instance.PlayCurtainSound(isAtClosedPosition, transform.position);
        
        if (SoundManager.Instance != null)
        {
            AudioClip clipToPlay = isAtClosedPosition ? openSound : closeSound;

            if (clipToPlay != null)
            {
                if (playSoundAtWorldPosition)
                {
                    SoundManager.Instance.PlaySoundAtPosition(clipToPlay, transform.position);
                }
                else
                {
                    SoundManager.Instance.PlaySoundGlobal(clipToPlay);
                }
            }
        }
        
        
        
        
    
        isAtClosedPosition = !isAtClosedPosition;

        Debug.Log($"{gameObject.name} sliding to {targetPosition} and scaling to {targetScale}");
    }

    // private IEnumerator AutoReturnAfterDelay()
    // {
    //     yield return new WaitForSeconds(autoReturnTime);
    //
    //     Debug.Log($"{gameObject.name} auto-return triggered.");
    //     ToggleCurtain();
    //
    //     autoReturnCoroutine = null;
    // }
}