using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] private float damage = 100f; // Damage to apply on touch
    private HealthManager playerHealth; // Reference to the player's HealthManager

    void Start()
    {
        // Find the HealthManager GameObject directly
        GameObject healthManagerObj = GameObject.Find("HealthManager");
        if (healthManagerObj != null)
        {
            playerHealth = healthManagerObj.GetComponent<HealthManager>();
        }

        // Error handling for missing references
        if (playerHealth == null)
        {
            Debug.LogError("HealthManager not found! Ensure the HealthManager GameObject exists and has a HealthManager component.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with a player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player
            playerHealth.TakeDamage(damage);
        }
    }
}
