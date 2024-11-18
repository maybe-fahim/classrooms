using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject endRoomPrefab;
    public List<GameObject> intermediateRoomPrefabs;
    public int numberOfIntermediateRooms = 5;
    public int randomSeed = 0; // Default seed is 0, meaning it will be randomized
    public List<GameObject> itemsToSpawn; // List of possible items to spawn
    public float itemSpawnChance = 0.5f; // Chance that an item spawns at each ItemAnchor

    private void Start()
    {
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        // Set the random seed for repeatable generation or use a truly random seed
        if (randomSeed == 0)
        {
            randomSeed = Random.Range(int.MinValue, int.MaxValue);
            Debug.Log("Generated random seed: " + randomSeed);
        }
        else
        {
            Debug.Log("Using set seed: " + randomSeed);
        }

        Random.InitState(randomSeed);  // Initialize random with the seed

        // List to keep track of generated rooms
        List<GameObject> generatedRooms = new List<GameObject>();

        // Instantiate and place the start room
        GameObject currentRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        generatedRooms.Add(currentRoom);

        // Get the last room's exit point for the initial room
        Transform lastExitAnchor = currentRoom.transform.Find("ExitAnchor");

        // Generate intermediate rooms
        for (int i = 0; i < numberOfIntermediateRooms; i++)
        {
            // Randomly pick an intermediate room prefab
            GameObject nextRoomPrefab = intermediateRoomPrefabs[Random.Range(0, intermediateRoomPrefabs.Count)];

            // Instantiate the next room
            GameObject nextRoom = Instantiate(nextRoomPrefab);
            generatedRooms.Add(nextRoom);

            // Align the entrance of the next room with the last room's exit
            Transform entranceAnchor = nextRoom.transform.Find("EntranceAnchor");
            if (entranceAnchor != null && lastExitAnchor != null)
            {
                // Calculate rotation that aligns the entrance with the last exit
                Quaternion targetRotation = Quaternion.LookRotation(lastExitAnchor.forward, lastExitAnchor.up);
                Quaternion entranceRotation = Quaternion.LookRotation(entranceAnchor.forward, entranceAnchor.up);
                Quaternion rotationOffset = targetRotation * Quaternion.Inverse(entranceRotation);

                nextRoom.transform.rotation = rotationOffset * nextRoom.transform.rotation;

                // Recalculate the position after rotation to align the entrance with the exit
                Vector3 entranceWorldPosition = nextRoom.transform.TransformPoint(entranceAnchor.localPosition);
                Vector3 positionOffset = lastExitAnchor.position - entranceWorldPosition;
                nextRoom.transform.position += positionOffset;

                // Update the last exit anchor to the current room's exit anchor for the next iteration
                lastExitAnchor = nextRoom.transform.Find("ExitAnchor");
            }
            else
            {
                Debug.LogWarning("EntranceAnchor or ExitAnchor not found on room prefab.");
            }

            // Handle Item Spawning in the intermediate room
            TrySpawnItemsInRoom(nextRoom);
        }

        // Instantiate and align the end room
        GameObject endRoom = Instantiate(endRoomPrefab);
        generatedRooms.Add(endRoom);

        Transform endEntranceAnchor = endRoom.transform.Find("EntranceAnchor");
        if (endEntranceAnchor != null && lastExitAnchor != null)
        {
            // Calculate rotation that aligns the entrance of the end room with the last exit
            Quaternion targetRotation = Quaternion.LookRotation(-lastExitAnchor.forward, lastExitAnchor.up);
            Quaternion entranceRotation = Quaternion.LookRotation(endEntranceAnchor.forward, endEntranceAnchor.up);
            Quaternion rotationOffset = targetRotation * Quaternion.Inverse(entranceRotation);

            endRoom.transform.rotation = rotationOffset * endRoom.transform.rotation;

            // Adjust position after rotation to match the last exit position
            Vector3 adjustedEntrancePosition = endRoom.transform.TransformPoint(endEntranceAnchor.localPosition);
            Vector3 positionOffset = lastExitAnchor.position - adjustedEntrancePosition;
            endRoom.transform.position += positionOffset;
        }
        else
        {
            Debug.LogWarning("EntranceAnchor not found on end room prefab.");
        }

        // Handle item spawning for the end room
        TrySpawnItemsInRoom(endRoom);

        Debug.Log("Room generation complete.");
    }

    // Function to randomly spawn items at all ItemAnchors in a room
    private void TrySpawnItemsInRoom(GameObject room)
    {
        // Find all ItemAnchors in the room
        Transform[] itemAnchors = room.GetComponentsInChildren<Transform>();
        List<Transform> itemAnchorList = new List<Transform>();

        // Filter for ItemAnchors based on name or specific component
        foreach (var anchor in itemAnchors)
        {
            if (anchor.name.Contains("ItemAnchor"))  // Assuming anchors are named "ItemAnchor"
            {
                itemAnchorList.Add(anchor);
            }
        }

        // Spawn items at each ItemAnchor with the chance
        foreach (Transform itemAnchor in itemAnchorList)
        {
            // Check if an item should spawn
            if (Random.Range(0f, 1f) <= itemSpawnChance)  // Based on the spawn chance
            {
                // Pick a random item from the list and instantiate it at the ItemAnchor
                GameObject randomItem = itemsToSpawn[Random.Range(0, itemsToSpawn.Count)];
                Instantiate(randomItem, itemAnchor.position, itemAnchor.rotation, room.transform);
            }
            else
            {
                Debug.Log($"No item spawned at {itemAnchor.name}.");
            }
        }
    }
}
