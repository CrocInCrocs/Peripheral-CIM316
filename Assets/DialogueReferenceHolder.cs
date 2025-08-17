using System;
using UnityEngine;

public class DialogueReferenceHolder : MonoBehaviour
{
    public BinChore binCheck;
    public DishChore dishCheck;
   
    public TypeWriter typeWriter;

    public GameObject backDoorText;
    public GameObject catFoodText;

    private void Start()
    {
        DialogueManager.Current.UpdateReferences(
            binCheck,
            dishCheck,
            typeWriter,
            backDoorText,
            catFoodText);
    }
}
