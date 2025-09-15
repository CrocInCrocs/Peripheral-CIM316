using UnityEngine;
using UnityEngine.UI;

public class FPController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] public bool holdToCompleteChore = true;
    [SerializeField] public bool enableChores = true;
    private ChoreProgressBar progressBar;
    private ChoreBase currentChore = null;


    #region Inspect Variables

    [Header("Inspect Settings")] public Canvas inspectCanvas;
    public KeyCode inspectKey = KeyCode.F;
    public GameObject inspectPanel;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public float inspectDistance = 1.5f;
    public Camera inspectCamera; // Assign your inspect camera here
    public float inspectRotationSpeed = 100f;
    public string inspectTag = "Inspect";

    private bool isInspecting = false;
    private Transform objectToInspect = null;
    private Vector3 previousMousePosition;

    #endregion

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;


    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;

    #endregion

    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;


    // for footstep sounds
    private float footstepTimer = 0f;
    public float footstepInterval = 100f; // Adjust as needed


    // Internal Variables
    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Sprint Bar
    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;

    // Internal Variables
    private CanvasGroup sprintBarCG;
    private bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion


    #region Pickup

    public KeyCode pickupKey = KeyCode.E;
    public float pickupRange = 2f;
    public Transform playerHandTransform;

    #endregion

    #region Drop

    public KeyCode dropKey = KeyCode.Q;
    public GameObject heldItem;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion

    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    void Start()
    {
        PeripheralGameManager.Current.SetFPController(this);
        if (inspectCamera != null)
        {
            inspectCamera.enabled = false; // Disable on start
        }

        progressBar = FindObjectOfType<ChoreProgressBar>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }

        #region Sprint Bar

        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        if (useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight = screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

            if (hideBarWhenFull)
            {
                sprintBarCG.alpha = 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }

        #endregion
    }


    float camRotation;

    private void Update()
    {
        #region Camera

        // Control camera movement
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        #region Camera Zoom

        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toogle zoom
            if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
            {
                if (!isZoomed)
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            // Changes isZoomed when key is pressed
            // Behavior for hold to zoom
            if (holdToZoom && !isSprinting)
            {
                if (Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if (Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed && !isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }

        #endregion

        #endregion

        #region Sprint

        if (enableSprint)
        {
            if (isSprinting)
            {
                isZoomed = false;
                playerCamera.fieldOfView =
                    Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

                // Drain sprint remaining while sprinting
                if (!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Time.deltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                // Regain sprint while not sprinting
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }

            // Handles sprint cooldown 
            // When sprint remaining == 0 stops sprint ability until hitting cooldown
            if (isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }

            // Handles sprintBar 
            if (useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
        }

        #endregion

        #region Jump

        // Gets input and calls jump method
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion


        #region UITextPromt

        if (!isInspecting && InventoryManager.Current.ReturnSelectedItemInInventory() == null)
        {
            GameObject target = RayCastFromCamera();

            if (target != null)
            {
                // Show appropriate prompt
                if (target.GetComponent<IPickupable>() != null)
                {
                    InteractionUI.Instance.ShowPrompt(InteractionUI.Instance.GetPickupInspectPrompt());
                }
                else
                {
                    InteractionUI.Instance.HidePrompt();
                }
            }
            else
            {
                InteractionUI.Instance.HidePrompt();
            }
        }

        #endregion
        

        #region Pickup

        if (!isInspecting)
        {
            if (Input.GetKeyDown(pickupKey))
            {
                GameObject target = RayCastFromCamera();

                if (target)
                {
                    // --- Prioritize chores ---
                    if (enableChores && currentChore == null)
                    {
                        IChoreable targetChore = target.GetComponent<IChoreable>();
                        if (targetChore != null)
                        {
                            ChoreBase chore = targetChore as ChoreBase;
                            if (chore != null)
                            {
                                currentChore = chore;
                                currentChore.StartChore();

                                if (progressBar != null)
                                {
                                    progressBar.SetChore(currentChore);
                                    Debug.Log("CHORE STARTED → SLIDER SET");
                                }

                                if (!holdToCompleteChore)
                                {
                                    currentChore = null;
                                }

                                return;
                            }
                        }
                    }

                    // --- If not a chore, check if it's an interactable ---
                    IInteractable interactable = target.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                        return;
                    }

                    // --- Then check for pickups ---
                    IPickupable pickupable = target.GetComponent<IPickupable>();
                    if (pickupable != null)
                    {
                        pickupable.Pickup(playerHandTransform);

                        // 🐱 Additional check for cat food
                        HandleCatFoodPickup(pickupable);

                        return;
                    }


                    // if (enableChores) // <- Only check for chores if enabled
                    // {
                    //     IChoreable chore = hit.collider.GetComponent<IChoreable>(); // Detect the choreable object
                    //     if (chore != null && currentChore == null) // Start the chore if none is active
                    //     {
                    //         currentChore = chore as ChoreBase;
                    //         currentChore.StartChore();
                    //
                    //         if (!holdToCompleteChore)
                    //         {
                    //             // If hold not required, immediately clear the chore reference
                    //             currentChore = null;
                    //         }
                    //
                    //         return;
                    //     }
                    // }

                    // IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    // if (interactable != null)
                    // {
                    //     interactable.Interact();
                    //     return;
                    // }
                }

              
            }

            if (!isInspecting && InventoryManager.Current.ReturnSelectedItemInInventory() != null)
            {
                InteractionUI.Instance.ShowPrompt(InteractionUI.Instance.GetDropPrompt());
            }
        }

        // Handling "hold to continue chore" separately!
        if (holdToCompleteChore && currentChore != null)
        {
            if (Input.GetKeyUp(pickupKey))
            {
                currentChore.StopChore();
                currentChore = null;
            }
        }

        #endregion


        #region Drop

        if (!isInspecting)
        {
            if (Input.GetKeyDown(dropKey))
            {
                heldItem = InventoryManager.Current.ReturnSelectedItemInInventory();
                // Throw/drop currently held item if there is one
                if (heldItem != null)
                {
                    IPickupable pickupable = heldItem.GetComponent<IPickupable>();
                    if (pickupable != null)
                    {
                        pickupable.Drop(playerHandTransform);
                        heldItem = null;
                        // ✅ Hide the prompt after drop
                        InteractionUI.Instance.HidePrompt();
                    }
                }
            }
        }

        #endregion


        #region Crouch

        if (enableCrouch)
        {
            if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                Crouch();
            }

            if (Input.GetKeyDown(crouchKey) && holdToCrouch)
            {
                isCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
            {
                isCrouched = true;
                Crouch();
            }
        }

        #endregion

        CheckGround();

        if (enableHeadBob && !isInspecting)
        {
            HeadBob();
        }

        #region Inspect

        if (Input.GetKeyDown(inspectKey))
        {
            if (isInspecting)
            {
                ExitInspectMode();
            }
            else
            {
                TryStartInspectMode();
            }
        }

        if (isInspecting)
        {
            RotateInspectedObject();
        }

        #endregion

        #region Inspect Functions

        void TryStartInspectMode()
        {
            GameObject target = RayCastFromCamera();

            // Replace tag check with IPickupable check or your own logic
            if (target != null && target.GetComponent<IPickupable>() != null)
            {
                isInspecting = true;
                objectToInspect = target.transform;

                if (inspectCanvas != null)
                    inspectCanvas.enabled = false;

                // Save original transform
                originalPosition = objectToInspect.position;
                originalRotation = objectToInspect.rotation;

                // Get Rigidbody and freeze physics
                Rigidbody rb = objectToInspect.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }


                // Move object into inspect view
                objectToInspect.position =
                    inspectCamera.transform.position + inspectCamera.transform.forward * inspectDistance;
                objectToInspect.rotation = Quaternion.identity;
                //
                // playerCamera.enabled = false;
                inspectCamera.enabled = true;

                // Show UI (if any)
                if (inspectPanel != null)
                    inspectPanel.SetActive(true);

                DisableInput();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Item item = target.GetComponent<Item>();
                if (item != null && DialogueManager.Current != null)
                {
                    DialogueManager.Current.NewText(item.dialogueText);
                }
            }
        }

        void ExitInspectMode()
        {
            if (objectToInspect != null)
            {
                Rigidbody rb = objectToInspect.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                // Reset transform
                objectToInspect.position = originalPosition;
                objectToInspect.rotation = originalRotation;

                objectToInspect = null;
            }

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            isInspecting = false;

            if (inspectCanvas != null)
                inspectCanvas.enabled = true;

            playerCamera.enabled = true;
            inspectCamera.enabled = false;

            if (inspectPanel != null)
                inspectPanel.SetActive(false); // ❌ Turn off panel

            EnableInput();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void RotateInspectedObject()
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && objectToInspect != null)
            {
                Vector3 deltaMouse = Input.mousePosition - previousMousePosition;

                float rotX = deltaMouse.y * inspectRotationSpeed * Time.deltaTime;
                float rotY = -deltaMouse.x * inspectRotationSpeed * Time.deltaTime;

                Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
                objectToInspect.rotation = rotation * objectToInspect.rotation;

                previousMousePosition = Input.mousePosition;
            }
        }

        #endregion
    }


    void FixedUpdate()
    {
        #region Movement

        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Checks if player is walking and isGrounded
            // Will allow head bob
            // if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;

            if ((targetVelocity.x != 0 || targetVelocity.z != 0) && isGrounded && horizontalVelocity.magnitude > 0.1f)
            {
                isWalking = true;

                // Play footstep sound
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    // SoundManager.Instance.PlayFootstepSound(transform.position);
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.PlayFootstepSound(transform.position);
                    }
                    else
                    {
                        Debug.LogWarning("SoundManager.Instance is null! Cannot play footstep sound.");
                    }

                    footstepTimer = footstepInterval;
                }
            }
            else
            {
                isWalking = false;
                footstepTimer = footstepInterval;
            }

            // All movement calculations shile sprint is active
            if (enableSprint && Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.linearVelocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    if (isCrouched)
                    {
                        Crouch();
                    }

                    if (hideBarWhenFull && !unlimitedSprint)
                    {
                        sprintBarCG.alpha += 5 * Time.deltaTime;
                    }
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;

                if (hideBarWhenFull && sprintRemaining == sprintDuration)
                {
                    sprintBarCG.alpha -= 3 * Time.deltaTime;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.linearVelocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        #endregion
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f),
            transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if (isCrouched && !holdToCrouch)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }

    private void HeadBob()
    {
        if (isWalking)
        {
            // Calculates HeadBob speed during sprint
            if (isSprinting)
            {
                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            // Calculates HeadBob speed during crouched movement
            else if (isCrouched)
            {
                timer += Time.deltaTime * (bobSpeed * speedReduction);
            }
            // Calculates HeadBob speed during walking
            else
            {
                timer += Time.deltaTime * bobSpeed;
            }

            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x,
                jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y,
                jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(
                Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed),
                Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed),
                Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }

    public GameObject RayCastFromCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            GameObject target = hit.collider.gameObject;
            return target;
        }

        return null;
    }


    // public IInteractable ReturnInteractableFromRayCast()

    public GameObject ReturnInteractableFromRayCast()

    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }

        return null;
    }


    // for computer 
    public void DisableInput()
    {
        playerCanMove = false;
        cameraCanMove = false;
    }

    public void EnableInput()
    {
        playerCanMove = true;
        cameraCanMove = true;
    }

    private void HandleCatFoodPickup(IPickupable pickupable)
    {
        GameObject obj = ((MonoBehaviour)pickupable).gameObject;

        if (!obj.CompareTag("CatFood")) return;

        Animator animator = obj.GetComponent<Animator>();
        if (animator == null) return;

        Collider[] nearbyTriggers = Physics.OverlapSphere(transform.position, 2f);
        foreach (var trigger in nearbyTriggers)
        {
            FeedCat feedTrigger = trigger.GetComponent<FeedCat>();
            if (feedTrigger != null)
            {
                feedTrigger.SetHeldCanAnimator(animator);
                Debug.Log("Cat food animator set on FeedCatTrigger.");
            }
        }
    }
}