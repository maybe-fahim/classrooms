using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    
    private Vector2 move;
    private CharacterController controller;


    // Reference to the camera
    public Camera playerCamera;
    public CameraShake cameraShake;

    // Movement variables
    public float acceleration = 10f;
    public float drag = 0.9f;
    private float maxSpeed = 5f;
    private Vector3 velocity = Vector3.zero; // Current velocity

    // Gravity and vertical velocity
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    // Crouch-related variables
    private bool isCrouching = false;
    private float originalMaxSpeed;
    private Vector3 originalScale;
    private float crouchMoveSpeed = 2.5f; // Slower movement when crouched
    private Vector3 crouchScale = new Vector3(0.5f, 0.5f, 0.5f); // Crouched size

    void Awake()
    {
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();

        // Save the original movement speed and player scale (initial size)
        originalMaxSpeed = maxSpeed;
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

        // Check if player is moving
        bool isMoving = move != Vector2.zero;

        // Trigger camera shake based on movement state
        if (isMoving)
        {
            cameraShake.StartShake();  // Start shake when moving
        }
        else
        {
            cameraShake.StopShake();   // Stop shake when idle
        }

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

        // Apply acceleration
        if (movement != Vector3.zero)
        {
            velocity += movement * acceleration * Time.deltaTime;
        }
        
        // Apply drag
        velocity *= drag;

        // Clamp velocity to max speed
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        // Apply gravity
        if (controller.isGrounded)
        {
            // Reset vertical velocity if grounded
            verticalVelocity = 0f;
        }
        else
        {
            // Apply gravity over time if not grounded
            verticalVelocity += gravity * Time.deltaTime;
        }

       // Add gravity to the movement
        velocity.y = verticalVelocity;

        // Move the player
        controller.Move(velocity * Time.deltaTime);

    }

    private void StartCrouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            maxSpeed = crouchMoveSpeed; // Reduce movement speed
            transform.localScale = crouchScale; // Shrink the player's size
        }
    }

    private void StopCrouch()
    {
        if (isCrouching)
        {
            isCrouching = false;
            maxSpeed = originalMaxSpeed; // Restore original movement speed
            transform.localScale = originalScale; // Restore the player's original size
        }
    }

}