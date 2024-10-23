using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public Transform player; // Reference to the player's Transform
    public float spawnDelay = 10f; // Time to wait before spawning an enemy
    private bool enemySpawned = false; // Flag to check if an enemy has been spawned

    private void Start()
    {
        // Start the spawn check coroutine
        StartCoroutine(CheckAndSpawnEnemy());
    }

    private IEnumerator CheckAndSpawnEnemy()
    {
        float idleTime = 0f;

        while (true)
        {
            if (enemySpawned)
            {
                // Wait for the enemy to be removed
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Enemy") == null);
                enemySpawned = false; // Reset the flag after the enemy is removed
            }

            // Check if the player has moved
            if (PlayerHasMoved())
            {
                idleTime = 0f; // Reset the idle time if the player moved
            }
            else
            {
                idleTime += Time.deltaTime; // Increase idle time
            }

            // If the player has been idle for the spawn delay, attempt to spawn an enemy
            if (idleTime >= spawnDelay && !enemySpawned)
            {
                SpawnEnemy();
                enemySpawned = true; // Set the flag to prevent additional spawns
            }

            yield return null; // Wait for the next frame
        }
    }

    private bool PlayerHasMoved()
    {
        // You can implement your logic to determine if the player has moved
        // For example, checking if the player’s position has changed significantly
        // Here’s a simple way to check for movement:
        return player.GetComponent<Rigidbody>().velocity.magnitude > 0.1f;
    }

    private void SpawnEnemy()
    {
        // Find the closest spawner to the player
        Transform closestSpawner = FindClosestSpawner();

        if (closestSpawner != null)
        {
            Instantiate(enemyPrefab, closestSpawner.position, closestSpawner.rotation);
        }
    }

    private Transform FindClosestSpawner()
    {
        Transform closestSpawner = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform spawner in transform)
        {
            float distance = Vector3.Distance(player.position, spawner.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpawner = spawner;
            }
        }

        return closestSpawner;
    }
}
