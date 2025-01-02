using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Bandage : MonoBehaviour
{
    [SerializeField] private InputActionReference useInput;
    private Hotbar hotbar; // Reference to the hotbar
    private HealthManager healthManager; // Reference to the HealthManager
    private bool hasUsedBandage; // Flag to track if bandage has been used

    void Start()
    {
        useInput.action.performed += UseBandage;

        hasUsedBandage = false;

        // Find the Hotbar instance in the scene
        hotbar = FindObjectOfType<Hotbar>();
        if (hotbar == null)
        {
            Debug.LogError("Hotbar not found in the scene.");
        }
        
        // Find the HealthManager instance in the scene
        healthManager = FindObjectOfType<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found in the scene.");
        }
    }

    void UseBandage(InputAction.CallbackContext obj)
    {
        if (hasUsedBandage)
        {
            return;
        }
        else
        {
            if (healthManager != null)
            {
                healthManager.heal(100f); // Heal the player up to 100 health points
                Debug.Log("Player bandaged and healed 100 health.");
                hasUsedBandage = true; // Set the flag to true
                // Remove the bandage item from the hotbar
                if (hotbar != null)
                {
                    hotbar.RemoveItemFromHotbar(itemType.Bandage); // Ensure itemType.Bandage exists
                }
            }
        }
        

        
    }
}
