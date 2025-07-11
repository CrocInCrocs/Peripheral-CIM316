using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCheckManager : MonoBehaviour
{
   
   public bool diningChecked, kitchenChecked, loungeChecked, garageChecked, bathroomChecked;
  

   public GameObject finalCutsceneTrigger;

   public void Update()
   {
      if (diningChecked && kitchenChecked && loungeChecked && garageChecked && bathroomChecked)
      {
         finalCutsceneTrigger.SetActive(true);
      }
   }
}
