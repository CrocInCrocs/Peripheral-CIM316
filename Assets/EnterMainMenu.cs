using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMainMenu : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(0);
    }
}
