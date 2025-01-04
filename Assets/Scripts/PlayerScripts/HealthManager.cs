using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("UI References")]
    public Image healthBar;
    
    [Header("Player References")]
    public GameObject player;
    public GameObject deathScreen;

    // We keep track of current health, and also of maxHP
    private float healthAmount = 100f;
    private float maxHP = 100f; // We'll override this from DifficultyManager

    void Awake()
    {
        // 1) Find the DifficultyManager in the scene
        var difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager != null)
        {
            // 2) Override local maxHP with the manager’s value
            maxHP = difficultyManager.GetPlayerMaxHP();
            Debug.Log("HealthManager: Overriding player max HP to " + maxHP);

            // 3) Set the player's current health to that maxHP
            healthAmount = maxHP;

            // 4) Initialize health bar fill (fully filled at start)
            if (healthBar != null)
            {
                healthBar.fillAmount = healthAmount / maxHP;
            }
        }
        else
        {
            Debug.LogWarning("HealthManager: No DifficultyManager found, using default HP = 100.");
            healthAmount = 100f;
        }
    }

    void Update()
    {
        // Check if the player is dead
        if (healthAmount <= 0)
        {
            // Disable movement
            if (player.TryGetComponent<PlayerMovement>(out var movement))
            {
                movement.enabled = false;
            }
            // Show death screen
            if (deathScreen != null) 
            {
                deathScreen.SetActive(true);
            }
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
        }

        // Ensure current health never exceeds maxHP
        if (healthAmount > maxHP)
        {
            healthAmount = maxHP;
        }

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHP;
        }
    }

    public void TakeDamage(float amount)
    {
        healthAmount -= amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHP);

        Debug.Log("Player Health: " + healthAmount);

        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHP;
        }
    }

    public void heal(float amount)
    {
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHP);

        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHP;
        }
    }
}
