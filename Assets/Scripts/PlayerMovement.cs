using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private float moveSpeed = 5f;
    private Vector2 move;
    private CharacterController controller;

    // Reference to the camera
    public Camera playerCamera;

    // Crouch-related variables
    private bool isCrouching = false;
    private float originalMoveSpeed;
    private Vector3 originalScale;
    private float crouchMoveSpeed = 2.5f; // Slower movement when crouched
    private Vector3 crouchScale = new Vector3(0.5f, 0.5f, 0.5f); // Crouched size

    void Awake()
    {
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();

        // Save the original movement speed and player scale (initial size)
        originalMoveSpeed = moveSpeed;
        originalScale = transform.localScale; // Save the actual starting scale
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void OnEnable()
    {
        controls.Enable();
        // Bind crouch action to the Crouch function
        controls.Player.Crouch.performed += _ => StartCrouch();
        controls.Player.Crouch.canceled += _ => StopCrouch();
    }

    private void OnDisable()
    {
        controls.Disable();

        // Unbind crouch action
        controls.Player.Crouch.performed -= _ => StartCrouch();
        controls.Player.Crouch.canceled -= _ => StopCrouch();
    }

    private void PlayerMove()
    {
        // Get input for movement (WASD or joystick)
        move = controls.Player.Move.ReadValue<Vector2>();

        // Get the forward and right directions from the camera
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        // Zero out the y component to keep the player grounded
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize the vectors (to prevent faster diagonal movement)
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction relative to the camera
        Vector3 movement = (cameraForward * move.y + cameraRight * move.x);

        // Move the player using the CharacterController
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void StartCrouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            moveSpeed = crouchMoveSpeed; // Reduce movement speed
            transform.localScale = crouchScale; // Shrink the player's size
        }
    }

    private void StopCrouch()
    {
        if (isCrouching)
        {
            isCrouching = false;
            moveSpeed = originalMoveSpeed; // Restore original movement speed
            transform.localScale = originalScale; // Restore the player's original size
        }
    }
}