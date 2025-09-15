using UnityEngine;
using DG.Tweening;

public class FeedCat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string requiredTag = "Player";
    [SerializeField] private string animatorBoolParameter = "EmptyCan";
    [SerializeField] private SphereCollider rangeTrigger;

    [Header("Food Settings")]
    [SerializeField] private GameObject foodObject; // The child object to toggle on/off

    [SerializeField] private GameObject playerInRange = null;
    [SerializeField] private Animator canAnimator = null;
    [SerializeField] private bool hasEmptied = false;
    private bool foodPresent = false;

    private void Start()
    {
        if (rangeTrigger == null)
            rangeTrigger = GetComponent<SphereCollider>();

        if (rangeTrigger != null)
            rangeTrigger.isTrigger = true;

        // Ensure food starts hidden
        if (foodObject != null)
            foodObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange != null && canAnimator != null && !hasEmptied && !foodPresent)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Feeding cat...");
                canAnimator.SetBool(animatorBoolParameter, true);
                hasEmptied = true;

                ShowFood();

           
                Debug.Log("Fed the cat.");
            }
        }
    }

    public void SetHeldCanAnimator(Animator animator)
    {
        canAnimator = animator;
        hasEmptied = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CatFood"))
        {
            playerInRange = other.gameObject;
            Animator animator = other.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                canAnimator = animator;
                Debug.Log("Cat food can detected in bowl range.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CatFood"))
        {
            playerInRange = null;
            Animator animator = other.GetComponentInChildren<Animator>();
            if (animator == canAnimator)
            {
                canAnimator = null;
                hasEmptied = false;
                Debug.Log("Cat food can left bowl range.");
            }
        }
    }

    private void ShowFood()
    {
        if (foodObject != null)
        {
            foodObject.SetActive(true);
            foodPresent = true;
            SoundManager.Instance.PLayCatFoodSound(transform.position);
            TaskEvents.InvokeChoreCompleted("Feed Cat");
        }
    }

    // 🔓 Call this from the cat script to "eat" the food
    public void CatEatFood()
    {
        if (foodObject != null && foodPresent)
        {
            foodObject.SetActive(false);
            foodPresent = false;
            Debug.Log("Cat has eaten the food. Bowl can be refilled.");
        }
    }
}