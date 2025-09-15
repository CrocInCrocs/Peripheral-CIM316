using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class computer : ChoreBase
{
    [SerializeField] private Camera[] cctvCameras;
    [SerializeField] private Canvas computerCanvas;
    [SerializeField] private Canvas tutorialCanvas;
    [SerializeField] private Canvas PlayerCanvas;
    [SerializeField] private Canvas InventoryCanvas;

    [SerializeField] private int currentCameraIndex = 0;
    [SerializeField] private bool isViewingCCTV = false;
    [SerializeField] private FPController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<FPController>();

        if (computerCanvas == null)
            computerCanvas = GameObject.Find("Computer Canvas")?.GetComponent<Canvas>();

        if (tutorialCanvas == null)
            tutorialCanvas = GameObject.Find("Tutorial Canvas")?.GetComponent<Canvas>();

        if (PlayerCanvas == null)
            PlayerCanvas = GameObject.Find("Player Canvas")?.GetComponent<Canvas>();

        if (InventoryCanvas == null)
            InventoryCanvas = GameObject.Find("Inventory Canvas")?.GetComponent<Canvas>();
    }

    // Enables the computer UI and disables player controls
    public void EnableComputerUI()
    {
        if (computerCanvas != null)
            computerCanvas.gameObject.SetActive(true);

        if (playerController != null)
        {
            playerController.DisableInput();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Enable the player's HUD canvas here:
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayComputerOnSound(playerController.transform.position);
        }

        Debug.Log("Started computer chore. Canvas enabled.");
    }

    // Starts the chore and enables the computer UI
    public override void StartChore()
    {
        base.StartChore();
        EnableComputerUI();
    }

    // Exits the computer, disables cameras and UI, and re-enables player controls
    public void ExitComputer()
    {
        Debug.Log("exited computer funtion called has been triggerecd, exiting computer");
        isViewingCCTV = false;

        if (computerCanvas != null)
            computerCanvas.gameObject.SetActive(false);

        foreach (var cam in cctvCameras)
            cam.gameObject.SetActive(false);

        // Enable PlayerCanvas  if assigned
        if (PlayerCanvas != null)
            PlayerCanvas.gameObject.SetActive(true);
        // Enable PlayerCanvas  if assigned
        if (InventoryCanvas != null)
            InventoryCanvas.gameObject.SetActive(true);


        if (playerController != null)
        {
            playerController.EnableInput();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Play computer sound at player position
        SoundManager.Instance.PlayComputerOffSound(playerController.transform.position);


        Debug.Log("Exited Computer");
    }

    // Activates the CCTV cameras and disables all other canvases except tutorial UI
    public void ActivateCameras()
    {
        // Disable all canvases in the scene
        // foreach (var canvas in FindObjectsOfType<Canvas>())
        //     canvas.gameObject.SetActive(false);


        if (computerCanvas != null)
            computerCanvas.gameObject.SetActive(false);

        // Enable tutorial canvas if assigned
        if (InventoryCanvas != null)
            InventoryCanvas.gameObject.SetActive(false);

        // Enable tutorial canvas if assigned
        if (tutorialCanvas != null)
            tutorialCanvas.gameObject.SetActive(true);

        if (playerController != null)
        {
            playerController.DisableInput();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (cctvCameras.Length == 0)
        {
            Debug.LogWarning("No CCTV cameras assigned!");
            return;
        }

        currentCameraIndex = 0;
        isViewingCCTV = true;
        ActivateCamera(currentCameraIndex);

        SoundManager.Instance.PlayCCTVLoopSound(playerController.transform.position);
        Debug.Log("ActivateCameras called: CCTV sound should play");
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Z)) // its a button for now, but we want it ot be on the screen for the compouter, so u can only log off from the desktop. 
        //     ExitComputer();

        if (!isViewingCCTV)
            return;

        HandleCameraCycleInput();
        HandleExitInput();
    }

    // Handles input for cycling through cameras
    private void HandleCameraCycleInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            SwitchCamera(1);
        else if (scroll < 0f)
            SwitchCamera(-1);

        if (Input.GetKeyDown(KeyCode.D))
            SwitchCamera(1);
        else if (Input.GetKeyDown(KeyCode.A))
            SwitchCamera(-1);
    }

    // Handles input for exiting the CCTV view
    private void HandleExitInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnableComputerUI();
            // Enable tutorial canvas if assigned
            if (tutorialCanvas != null)
                tutorialCanvas.gameObject.SetActive(false);

            SoundManager.Instance.StopCCTVLoopSound();
            Debug.Log("CCTV sound stopped on exit");
        }
    }


    // Switches to a different camera index
    private void SwitchCamera(int direction)
    {
        cctvCameras[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex = (currentCameraIndex + direction + cctvCameras.Length) % cctvCameras.Length;
        ActivateCamera(currentCameraIndex);
        // ✅ Play camera switch sound
        SoundManager.Instance.SwitchCameraSound(playerController.transform.position);
        Debug.Log("SwitchCamera sound called");
    }

    // Activates the camera at the given index and disables the others
    private void ActivateCamera(int index)
    {
        for (int i = 0; i < cctvCameras.Length; i++)
            cctvCameras[i].gameObject.SetActive(i == index);
    }
}