using System;
using UnityEngine;

public class DialogueHolder : DialogueBase
{
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasBeenPlayed != true)
            {
                DialogueManager.Current.NewText(dialogueText);
                hasBeenPlayed = true;
            }
        }
        
        
    }
}
