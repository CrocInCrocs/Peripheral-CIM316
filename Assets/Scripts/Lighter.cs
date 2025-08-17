using System;
using UnityEngine;

public class Lighter : MonoBehaviour
{
   public Animation lighterOpen;
   public void OnEnable(Animation lighterOpen)
   {
      lighterOpen.Play();
   }
}
