using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnDelay = 10f;
    public float spawnCooldown = 5f;
    private bool enemySpawned = false;
    private float lastSpawnTime;

    private void Start()
    {
        StartCoroutine(CheckAndSpawnEnemy());
    }

    private IEnumerator CheckAndSpawnEnemy()
    {
        float idleTime = 0f;

        while (true)
        {
            if (enemySpawned)
            {
                yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Enemy") == null);
                enemySpawned = false;
                lastSpawnTime = Time.time;
            }

            if (PlayerHasMoved())
            {
                idleTime = 0f;
            }
            else
            {
                idleTime += Time.deltaTime;
            }

            if (idleTime >= spawnDelay && !enemySpawned && Time.time - lastSpawnTime >= spawnCooldown)
            {
                SpawnEnemy();
                enemySpawned = true;
            }

            yield return null;
        }
    }

    private bool PlayerHasMoved()
    {
        return player.GetComponent<Rigidbody>().velocity.magnitude > 0.1f;
    }

    private void SpawnEnemy()
    {
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
