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
            HealthManager php = other.GetComponent<HealthManager>();

            if (php != null)
            {
                // Reduce the player's health
                php.TakeDamage(damageAmount);

                // Destroy the enemy game object
                Destroy(gameObject);
            }
        }
    }
}
