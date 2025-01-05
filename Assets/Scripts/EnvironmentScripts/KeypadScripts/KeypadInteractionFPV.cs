using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Import Unity's new Input System namespace

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        public Camera cam;

        [SerializeField] private InputActionReference useInput; // Reference to the InputAction

        private void Awake()
        {
            if (cam == null)
            {
                Debug.LogError("Main Camera not assigned. Ensure you have assigned a Camera in the Inspector.");
            }
        }

        private void OnEnable()
        {
            if (useInput != null)
            {
                // Subscribe to the input action
                useInput.action.performed += OnUseInput;
            }
        }

        private void OnDisable()
        {
            if (useInput != null)
            {
                // Unsubscribe from the input action
                useInput.action.performed -= OnUseInput;
            }
        }

        private void OnUseInput(InputAction.CallbackContext context)
        {
            if (cam == null) return; // Ensure the camera is valid

            // Create a ray from the camera through the mouse position
            var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<KeypadButton>(out KeypadButton keypadButton))
                {
                    keypadButton.PressButton(); // Call the button press action
                    Debug.Log($"Pressed button: {keypadButton.name}");
                }
            }
        }
    }
}