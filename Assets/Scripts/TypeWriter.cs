using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    public string textToWrite = "";
    private string tempString = "";
    public TextMeshProUGUI text;
    public float textSpeed;
    private bool textPrinted;
    public float fadeTimer;
    private float _timer;
    
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponent<TextMeshProUGUI>();
    }

    private IEnumerator PrintText()
    {
        text.text = "";
        canvasGroup.alpha = 1;
        textPrinted = false;
        for (int i = 0; i < textToWrite.Length+1; i++)
        {
            tempString = textToWrite.Substring(0, i);
            text.text = tempString;
            yield return new WaitForSeconds(textSpeed);
        }
        textPrinted = true;
        _timer = fadeTimer;
    }

    public void StartCustomText(string TextToWrite)
    {
        StopCoroutine(PrintText());
        textToWrite = TextToWrite;
        StartCoroutine(PrintText());
    }
    public void Update()
    {
        if (canvasGroup == null) return;
        if (!textPrinted)
        {
            canvasGroup.alpha = 1;
            return;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        canvasGroup.alpha = _timer / fadeTimer;
    }
}
