using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject itemPrefab;
        [Range(0, 1)] public float spawnChance; // Chance to spawn this item (0 to 1)
    }

    [Header("Spawn Settings")]
    [Range(0, 1)] public float overallSpawnChance = 0.5f; // Overall chance for any item to spawn
    public List<SpawnableItem> itemsToSpawn;
    public Vector3 counterScale = Vector3.one; // Counter scale to negate parent's scaling

    [Header("Physics Settings")]
    public bool applyGravityOnSpawn = true; // Should the spawned item fall?

    void Start()
    {
        TrySpawnItem();
    }

    void TrySpawnItem()
    {
        // Determine if an item should spawn at all
        if (Random.value > overallSpawnChance)
        {
            Debug.Log("No item spawned: overall spawn chance failed.");
            return;
        }

        // Filter items based on individual spawn chances
        List<SpawnableItem> validItems = new List<SpawnableItem>();

        foreach (var item in itemsToSpawn)
        {
            if (Random.value <= item.spawnChance)
            {
                validItems.Add(item);
            }
        }

        if (validItems.Count == 0)
        {
            Debug.Log("No item spawned: no valid items passed their spawn chance.");
            return;
        }

        // Randomly select one of the valid items
        SpawnableItem selectedItem = validItems[Random.Range(0, validItems.Count)];

        // Spawn the item at the anchor's position
        GameObject spawnedItem = Instantiate(selectedItem.itemPrefab, transform.position, Quaternion.identity, transform);

        // Apply counter scale to negate parent's scaling
        spawnedItem.transform.localScale = Vector3.Scale(Vector3.one, new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z));

        // Ensure the item falls naturally
        if (applyGravityOnSpawn && spawnedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        Debug.Log($"Spawned item: {selectedItem.itemPrefab.name}");
    }
}
