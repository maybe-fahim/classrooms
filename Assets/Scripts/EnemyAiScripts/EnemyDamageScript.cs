using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt to the player

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the player
        if (other.CompareTag("Player"))
        {
            // Attempt to get the HealthManager component from the player
            HealthManager healthManager = other.GetComponent<HealthManager>();

            if (healthManager != null)
            {
                // Apply damage to the player's health
                healthManager.TakeDamage(damageAmount);
            }
        }
    }
}

