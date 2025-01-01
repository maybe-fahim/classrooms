using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField] private InputActionReference useInput;
    private Hotbar hotbar; // Reference to the hotbar
    private HealthManager healthManager; // Reference to the HealthManager
    private bool hasUsedWater; // Flag to track if water has been used

    void Start()
    {
        useInput.action.performed += DrinkWater;

        hasUsedWater = false;

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

    void DrinkWater(InputAction.CallbackContext obj)
    {
        if (hasUsedWater)
        {
            return;
        }
        if (healthManager != null)
        {
            healthManager.heal(10f); // Heal the player by 10 health points
            Debug.Log("Player drank water and healed 10 health.");
            hasUsedWater = true; // Set the flag to true
            // Remove the drink item from the hotbar
            if (hotbar != null)
            {
                hotbar.RemoveItemFromHotbar(itemType.Water); // Ensure itemType.Drink exists
            }
        }


        
    }
}
