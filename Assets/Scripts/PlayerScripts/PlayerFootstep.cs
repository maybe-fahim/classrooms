using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFootstep : MonoBehaviour
{
    public AudioSource footstepAudio;
    public AudioClip footstepClip; // Footstep sound clip
    public float footstepInterval = 0.5f; // Time between footsteps
    private float footstepTimer;

    private bool isCrouching = false; // Track if the player is crouching

    private PlayerControls controls;

    public delegate void OnFootstep(Vector3 position, bool isCrouching);
    public static event OnFootstep FootstepEvent;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Get the crouch input state from the new input system
        isCrouching = controls.Player.Crouch.ReadValue<float>() > 0.1f; // If the crouch action is pressed

        // Get movement input (check if player is moving using the new input system)
        Vector2 movement = controls.Player.Move.ReadValue<Vector2>();
        bool isMoving = movement != Vector2.zero;

        // Check if the player is moving and not crouching
        if (isMoving && !isCrouching)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstepSound();
                footstepTimer = 0f;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepAudio != null && footstepClip != null)
        {
            footstepAudio.PlayOneShot(footstepClip); // Play footstep sound
            FootstepEvent?.Invoke(transform.position, isCrouching); // Trigger the footstep event
        }
    }
}
