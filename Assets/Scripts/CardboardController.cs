using UnityEngine;

public class CardboardController : ChoreBase
{
    [SerializeField] private Animator cardboardAnimator;
    [SerializeField] private bool isOpen = false;
    
    private Collider[] childColliders;

    private void Awake()
    {

        childColliders = GetComponentsInChildren<Collider>(includeInactive: true);


        foreach (Collider col in childColliders)
        {
            if (col.gameObject != this.gameObject)
                col.enabled = false;
        }
    }

    public override void CompleteChore()
    {
        base.CompleteChore();

        if (cardboardAnimator != null)
        {
            if (!isOpen)
            {
                // Open the box
                cardboardAnimator.SetBool("Open", true);
                cardboardAnimator.SetBool("Close", false);
                Debug.Log("Cardboard opened.");

    
                SoundManager.Instance.PlaySound(SoundType.CardboardOpen, transform.position);

                foreach (Collider col in childColliders)
                {
                    if (col.gameObject != this.gameObject)
                        col.enabled = true;
                }
            }
            else
            {
                // Close the box
                cardboardAnimator.SetBool("Close", true);
                cardboardAnimator.SetBool("Open", false);
                Debug.Log("Cardboard closed.");
                

                SoundManager.Instance.PlaySound(SoundType.CardboardClose, transform.position);

                


                foreach (Collider col in childColliders)
                {
                    if (col.gameObject != this.gameObject)
                        col.enabled = false;
                }
            }

            isOpen = !isOpen; 
        }
    }
}