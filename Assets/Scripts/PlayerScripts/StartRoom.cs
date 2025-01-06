using System.Collections;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player GameObject

    private void Start()
    {
        // Start the coroutine to teleport the player after 0.5 seconds
        StartCoroutine(TeleportPlayerToSpawnPoint());
    }

    private IEnumerator TeleportPlayerToSpawnPoint()
    {
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Find the PlayerSpawnPoint in the scene
        GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint");

        if (spawnPoint != null && player != null)
        {
            // Teleport the player to the spawn point's position and rotation
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;

            Debug.Log("Player teleported to PlayerSpawnPoint.");
        }
        else
        {
            Debug.LogError("PlayerSpawnPoint or player reference not found in the scene.");
        }
    }
}