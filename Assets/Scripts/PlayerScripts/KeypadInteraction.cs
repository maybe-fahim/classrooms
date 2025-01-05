using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadInteraction : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionReference interactionInput; // Reference to the interact action

    [Header("Cameras")]
    public Camera playerCam; // Player's camera
    public Camera keypadCam; // Keypad's camera

    [Header("UI Elements")]
    public GameObject Reticle; // Reticle UI
    public GameObject UseKeyPadUI; // Use Keypad UI (button)
    

    [Header("Interaction Settings")]
    public float interactionRange = 5f; // Range for detecting the keypad

    private bool isUsingKeypad = false; // Tracks whether the player is interacting with the keypad

    private void Start()
    {
        InitializeStates();
    }

    private void OnEnable()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed += HandleInteract;
        }
    }

    private void OnDisable()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed -= HandleInteract;
        }
    }

    private void InitializeStates()
    {
        // Ensure initial states are correct
        if (playerCam != null) playerCam.gameObject.SetActive(true);
        if (keypadCam != null) keypadCam.gameObject.SetActive(false);
        if (Reticle != null) Reticle.SetActive(true);
        if (UseKeyPadUI != null) UseKeyPadUI.SetActive(false);
        

        // Lock and hide the cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isUsingKeypad)
        {
            UpdateUseKeypadUI();
        }
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (isUsingKeypad)
        {
            ExitKeypad();
        }
        else if (RaycastForKeypad())
        {
            UseKeypad();
        }
    }

    private void UpdateUseKeypadUI()
    {
        if (RaycastForKeypad())
        {
            if (UseKeyPadUI != null) UseKeyPadUI.SetActive(true);
        }
        else
        {
            if (UseKeyPadUI != null) UseKeyPadUI.SetActive(false);
        }
    }

    private bool RaycastForKeypad()
    {
        if (playerCam == null) return false;

        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.CompareTag("Keypad"))
            {
                return true;
            }
        }
        return false;
    }

    private void UseKeypad()
    {
        isUsingKeypad = true;
        if (UseKeyPadUI != null) UseKeyPadUI.SetActive(false);
        // Switch cameras
        SwitchCamera(keypadCam, playerCam);

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Update UI
        if (Reticle != null) Reticle.SetActive(false);
        

        Debug.Log("Using Keypad");
    }

    public void ExitKeypad()
    {
        isUsingKeypad = false;

        // Switch cameras
        SwitchCamera(playerCam, keypadCam);

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Update UI
        if (Reticle != null) Reticle.SetActive(true);
        

        Debug.Log("Exited Keypad");
    }

    private void SwitchCamera(Camera activateCam, Camera deactivateCam)
    {
        if (deactivateCam != null) deactivateCam.gameObject.SetActive(false);
        if (activateCam != null) activateCam.gameObject.SetActive(true);
    }
}
