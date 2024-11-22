using UnityEngine;

public class EnemyLookDetection : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Damage to apply per second
    private float damageTimer = 0f;

    private Transform playerCamera; // Reference to the player's camera
    private HealthManager playerHealth; // Reference to the HealthManager
    private LayerMask playerLayerMask; // Layer mask to ignore the player's collider

    void Start()
    {
        // Find the player's camera dynamically
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerCamera = player.transform.Find("Head/Camera");
        }

        // Find the HealthManager GameObject directly
        GameObject healthManagerObj = GameObject.Find("HealthManager");
        if (healthManagerObj != null)
        {
            playerHealth = healthManagerObj.GetComponent<HealthManager>();
        }

        // Create a layer mask that ignores the player’s collider
        playerLayerMask = ~LayerMask.GetMask("Player"); // Assuming "Player" is the layer for the player

        // Error handling for missing references
        if (playerCamera == null)
        {
            Debug.LogError("Player camera not found! Ensure the player's camera is correctly named and in the hierarchy.");
        }

        if (playerHealth == null)
        {
            Debug.LogError("HealthManager not found! Ensure the HealthManager GameObject exists and has a HealthManager component.");
        }
    }

    void Update()
    {
        if (playerCamera == null || playerHealth == null) return;

        if (IsPlayerLookingAtEnemy())
        {
            // Increment timer
            damageTimer += Time.deltaTime;

            // Apply damage every second
            if (damageTimer >= 1f)
            {
                playerHealth.TakeDamage(damage);
                damageTimer = 0f; // Reset the timer
            }
        }
        else
        {
            // Reset timer if the player is not looking
            damageTimer = 0f;
        }
    }

    private bool IsPlayerLookingAtEnemy()
    {
        // Calculate direction to the enemy
        Vector3 directionToEnemy = transform.position - playerCamera.position;

        // Check if the player is looking within the field of view
        float angle = Vector3.Angle(playerCamera.forward, directionToEnemy);

        if (angle < 45f) // Adjust this value for a wider or narrower field of view
        {
            // Check if there is a direct line of sight, ignoring the player's collider
            if (!Physics.Linecast(playerCamera.position, transform.position, playerLayerMask))
            {
                return true; // Player is looking at the enemy
            }
        }
        return false;
    }
}
