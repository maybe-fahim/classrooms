using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Amount of damage the enemy will deal to the player
    public int damageAmount = 1;

    // This function is called when the enemy collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object it collided with is the player
        if (other.CompareTag("Player"))
        {
            // Access the player's health component and reduce health
            Health playerHealth = other.GetComponent<Health>();

            if (playerHealth != null)
            {
                // Reduce the player's health
                playerHealth.TakeDamage(damageAmount);

                // Destroy the enemy game object
                Destroy(gameObject);
            }
        }
    }
}
