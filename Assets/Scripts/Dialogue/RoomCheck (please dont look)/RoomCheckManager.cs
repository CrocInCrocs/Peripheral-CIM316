using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomCheckManager : MonoBehaviour
{
   #region EventBus
   private static RoomCheckManager _current;
   public static RoomCheckManager Current { get { return _current; } }

   private void Awake()
   {
      if (_current != null && _current != this)
      {
         Destroy(this.gameObject);
      } else {
         _current = this;
         DontDestroyOnLoad(gameObject);
      }
   }
   #endregion
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
