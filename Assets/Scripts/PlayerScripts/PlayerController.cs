using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;

    [Header("Configurations")]
    public float walkSpeed;

    [Header("Crouching")]
    public float crouchSpeed;
    private Vector3 crouchScale = new Vector3(0.5f, 0.5f, 0.5f);
    private float startYScale;
    private float startWalkSpeed;

    private Vector2 moveInput; // Store movement input from the new input system
    private Vector2 lookInput; // Store look input from the new input system
    private bool isCrouching = false;

    // Input Actions
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startYScale = transform.localScale.y;
        startWalkSpeed = walkSpeed;

        // Get the InputActions from the PlayerInput component
        var playerInput = GetComponent<PlayerInput>();

        // Assign actions
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        crouchAction = playerInput.actions["Crouch"];

        // Subscribe to crouch actions
        crouchAction.started += OnCrouchStart;
        crouchAction.canceled += OnCrouchEnd;

        moveAction.Enable();
        lookAction.Enable();
        crouchAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Get movement input
        moveInput = moveAction.ReadValue<Vector2>(); // Read movement input from the action

        // Horizontal rotation (Yaw)
        Vector3 rotation = Vector3.up * lookInput.x * 2f;
        transform.Rotate(rotation);
    }

    void LateUpdate()
    {
        // Get the look input (Mouse movement)
        lookInput = lookAction.ReadValue<Vector2>();

        // Vertical rotation (Pitch)
        Vector3 e = head.eulerAngles;
        e.x -= lookInput.y * 2f;
        e.x = RestrictAngle(e.x, -85f, 85f);
        head.eulerAngles = e;
    }

    void FixedUpdate()
    {
        // Movement (forward/backward, left/right) based on input
        Vector3 newVelocity = Vector3.up * rb.velocity.y; // Retain Y velocity (gravity)
        newVelocity.x = moveInput.x * walkSpeed;
        newVelocity.z = moveInput.y * walkSpeed;
        rb.velocity = transform.TransformDirection(newVelocity);
    }

    void OnCrouchStart(InputAction.CallbackContext context)
    {
        // Crouching when the Left Control is pressed
        transform.localScale = crouchScale;
        walkSpeed = crouchSpeed;
        isCrouching = true;
    }

    void OnCrouchEnd(InputAction.CallbackContext context)
    {
        // Reset to normal scale when Left Control is released
        transform.localScale = new Vector3(1, startYScale, 1);
        walkSpeed = startWalkSpeed;
        isCrouching = false;
    }

    public static float RestrictAngle(float angle, float angleMin, float angleMax)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        if (angle > angleMax)
        {
            angle = angleMax;
        }
        else if (angle < angleMin)
        {
            angle = angleMin;
        }
        return angle;
    }

    void OnEnable()
    {
        // Enable input actions
        moveAction.Enable();
        lookAction.Enable();
        crouchAction.Enable();
    }

    void OnDisable()
    {
        // Disable input actions
        moveAction.Disable();
        lookAction.Disable();
        crouchAction.Disable();
    }
}