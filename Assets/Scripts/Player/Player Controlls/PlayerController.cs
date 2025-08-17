
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    public FPController fpController;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions["Look"].performed += OnLookPerformed;
    }

    void OnDisable()
    {
        playerInput.actions["Look"].performed -= OnLookPerformed;
    }

    void OnLookPerformed(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        // Use mouseDelta for camera rotation or other look functionality
        fpController.MouseYMovement(mouseDelta.y);
        fpController.MouseXMovement(mouseDelta.x);
    }
}
