using UnityEngine;

public class CameraLookAtDamage : MonoBehaviour
{
    public Transform playerHeadCamera; // Reference to the player's camera inside their head (Head/Camera)
    public Transform enemyObjectTransform; // Reference to the enemy object (not a camera, just an object)
    public float damageAmount = 10f; // Amount of damage per second
    public float damageInterval = 1f; // Interval for damage application
    public float raycastDistance = 50f; // Max distance for raycast to detect the enemy object

    private float damageTimer = 0f;

    void Update()
    {
        // Cast a ray from the player's head camera to check if it's looking at the enemy object
        RaycastHit hit;
        if (IsLookingAtEnemyObject(out hit))
        {
            damageTimer += Time.deltaTime;

            // Apply damage every damageInterval seconds
            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f; // Reset timer after applying damage
            }
        }
        else
        {
            damageTimer = 0f; // Reset timer if not looking at the enemy object
        }
    }

    // Check if the player's camera inside their head is looking at the enemy object
    bool IsLookingAtEnemyObject(out RaycastHit hit)
    {
        Vector3 rayDirection = enemyObjectTransform.position - playerHeadCamera.position; // Direction from head camera to enemy object
        return Physics.Raycast(playerHeadCamera.position, rayDirection.normalized, out hit, raycastDistance);
    }

    // Apply damage to the player using the HealthManager component
    void ApplyDamage()
    {
        // Assuming the player has a HealthManager script with a TakeDamage method
        playerHeadCamera.GetComponentInParent<HealthManager>().TakeDamage(damageAmount);
    }
}
