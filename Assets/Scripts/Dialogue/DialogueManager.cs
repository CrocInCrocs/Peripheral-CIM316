using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region EventBus
    private static DialogueManager _current;
    public static DialogueManager Current { get { return _current; } }

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
   
    public BinChore binCheck; 
   
    public TypeWriter typeWriter;

    public GameObject backDoorText;
    
    public void NewText(string dialogueText)
    {
        typeWriter.StartCustomText(dialogueText);
    }

    public void Update()
    {
        if (binCheck.binChoreCheck)
        {
            backDoorText.SetActive(true);
        }
    }
}
