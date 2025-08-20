using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;



public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private FPController FPController; // not MonoBehaviour

    private bool isPaused = false;
    // private float previousVolume;
    
    [SerializeField] private computer computerInstance;

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
            FPController.DisableInput();

        if (videoPlayer != null)
            videoPlayer.Pause();

        // // Pause all audio
        // previousVolume = AudioListener.volume;
        // AudioListener.volume = 0f;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
        
        // Only lock cursor if computer is NOT active
        if (computerInstance != null && computerInstance.IsComputerActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (FPController != null)
            FPController.EnableInput();
        
        if (videoPlayer != null)
            videoPlayer.Play();

        // // Resume all audio
        // AudioListener.volume = previousVolume;

        Time.timeScale = 1f;
    }
}