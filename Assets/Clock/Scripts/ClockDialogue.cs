using UnityEngine;

public class ClockDialogue : DialogueBase, IInteractable
{
    public void Interact()
    {
        throw new System.NotImplementedException();
    }


public FPController player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        player = PeripheralGameManager.Current.returnFPController();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null && PeripheralGameManager.Current != null)
        {
            player = PeripheralGameManager.Current.returnFPController();
        }

        GameObject interactable = player.ReturnInteractableFromRayCast();
        if (interactable == gameObject)
        {
            if (hasBeenPlayed != true)
            {
                DialogueManager.Current.NewText(dialogueText);
                hasBeenPlayed = true;
                
            }
        }
        
    }
}
