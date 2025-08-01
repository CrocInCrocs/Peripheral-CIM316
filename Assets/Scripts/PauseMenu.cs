using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;



public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private MonoBehaviour FPController; // Drag your FPController script here
    [SerializeField] private VideoPlayer videoPlayer; 

    private GameObject pauseMenuInstance;
    public bool isPaused = false;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (FPController != null)
            FPController.enabled = false;
        
        if (videoPlayer != null)
            videoPlayer.Pause();
        
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (FPController != null)
            FPController.enabled = true;
        
        if (videoPlayer != null)
            videoPlayer.Play();
        
        Time.timeScale = 1f;
    }
}