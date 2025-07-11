using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCheckManager : DialogueBase
{
   
   public bool diningChecked, kitchenChecked, loungeChecked, garageChecked, bathroomChecked;
   public bool textPlayed;
  

   public GameObject finalCutsceneTrigger;

   public void Update()
   {
      if (diningChecked && kitchenChecked && loungeChecked && garageChecked && bathroomChecked)
      {
         finalCutsceneTrigger.SetActive(true);
         if (textPlayed == false) 
         {
            DialogueManager.Current.NewText(dialogueText);
            textPlayed = true;
         }
         
      }
   }
}
