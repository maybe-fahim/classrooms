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

    private List<GameObject> generatedRooms = new List<GameObject>();
    private int currentRoomIndex = 0;

    private void Start()
    {
        GenerateInitialRooms();
        SpawnPlayer();
    }

    private void GenerateInitialRooms()
    {
        if (randomSeed == 0)
        {
            randomSeed = Random.Range(int.MinValue, int.MaxValue);
            Debug.Log("Generated random seed: " + randomSeed);
        }
        else
        {
            Debug.Log("Using set seed: " + randomSeed);
        }

        Random.InitState(randomSeed); // Initialize random with the seed

        GameObject currentRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        generatedRooms.Add(currentRoom);

        Transform lastExitAnchor = currentRoom.transform.Find("ExitAnchor");

        for (int i = 0; i < 3; i++) // Generate first 3 intermediate rooms
        {
            GameObject nextRoom = GenerateRoom(lastExitAnchor);
            lastExitAnchor = nextRoom.transform.Find("ExitAnchor");
        }
    }

    private GameObject GenerateRoom(Transform lastExitAnchor)
    {
        GameObject nextRoomPrefab = intermediateRoomPrefabs[Random.Range(0, intermediateRoomPrefabs.Count)];
        GameObject nextRoom = Instantiate(nextRoomPrefab);
        generatedRooms.Add(nextRoom);

        Transform entranceAnchor = nextRoom.transform.Find("EntranceAnchor");
        if (entranceAnchor != null && lastExitAnchor != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastExitAnchor.forward, lastExitAnchor.up);
            Quaternion entranceRotation = Quaternion.LookRotation(entranceAnchor.forward, entranceAnchor.up);
            Quaternion rotationOffset = targetRotation * Quaternion.Inverse(entranceRotation);

            nextRoom.transform.rotation = rotationOffset * nextRoom.transform.rotation;

            Vector3 entranceWorldPosition = nextRoom.transform.TransformPoint(entranceAnchor.localPosition);
            Vector3 positionOffset = lastExitAnchor.position - entranceWorldPosition;
            nextRoom.transform.position += positionOffset;

            // Attach trigger logic to the OpenDoorTrigger
            Transform doorTrigger = nextRoom.transform.Find("DoorModel2/OpenDoorTrigger");
            if (doorTrigger != null)
            {
                Collider triggerCollider = doorTrigger.GetComponent<Collider>();
                if (triggerCollider != null)
                {
                    triggerCollider.isTrigger = true;
                    doorTrigger.gameObject.AddComponent<OpenDoorTrigger>().Initialize(this, generatedRooms.Count - 1);
                }
            }
        }
        else
        {
            Debug.LogWarning("EntranceAnchor or ExitAnchor not found on room prefab.");
        }

        TrySpawnItemsInRoom(nextRoom);
        return nextRoom;
    }

    private void SpawnPlayer()
    {
        // Find the player object in the scene
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && generatedRooms.Count > 0)
        {
            GameObject startRoom = generatedRooms[0];
            Transform playerSpawnPoint = startRoom.transform.Find("PlayerSpawnPoint");

            if (playerSpawnPoint != null)
            {
                // Teleport the player to the spawn point
                player.transform.position = playerSpawnPoint.position;
                player.transform.rotation = playerSpawnPoint.rotation;

                Debug.Log("Player teleported to the start room's spawn point.");
            }
            else
            {
                Debug.LogWarning("PlayerSpawnPoint not found in the start room.");
            }
        }
        else
        {
            if (player == null)
            {
                Debug.LogWarning("Player object not found in the scene. Ensure it is tagged as 'Player'.");
            }
            if (generatedRooms.Count == 0)
            {
                Debug.LogWarning("No generated rooms available to locate the spawn point.");
            }
        }
    }

    public void OnPlayerEnterRoom(int roomIndex)
    {
        currentRoomIndex = roomIndex;

        // Generate the next intermediate room if applicable
        if (roomIndex + 3 < numberOfIntermediateRooms + 1 && roomIndex + 3 >= generatedRooms.Count)
        {
            Transform lastExitAnchor = generatedRooms[generatedRooms.Count - 1].transform.Find("ExitAnchor");
            GenerateRoom(lastExitAnchor);
        }

        // Check if we need to generate the end room when approaching the final room
        if (roomIndex == numberOfIntermediateRooms - 1 && !generatedRooms.Contains(endRoomPrefab))
        {
            Transform lastExitAnchor = generatedRooms[generatedRooms.Count - 1].transform.Find("ExitAnchor");
            GenerateEndRoom(lastExitAnchor);
        }

        // Delete old rooms to manage memory
        if (currentRoomIndex >= 5)
        {
            int roomToDeleteIndex = currentRoomIndex - 5;
            if (roomToDeleteIndex >= 0 && roomToDeleteIndex < generatedRooms.Count)
            {
                if (generatedRooms[roomToDeleteIndex] != null)
                {
                    Destroy(generatedRooms[roomToDeleteIndex]);
                    generatedRooms[roomToDeleteIndex] = null;
                }
            }
        }
    }

    private void GenerateEndRoom(Transform lastExitAnchor)
    {
        if (lastExitAnchor == null)
        {
            Debug.LogWarning("No exit anchor found for the last intermediate room.");
            return;
        }

        GameObject endRoom = Instantiate(endRoomPrefab);
        generatedRooms.Add(endRoom);

        Transform entranceAnchor = endRoom.transform.Find("EntranceAnchor");
        if (entranceAnchor != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-lastExitAnchor.forward, lastExitAnchor.up); // Reverse the direction
            Quaternion entranceRotation = Quaternion.LookRotation(entranceAnchor.forward, entranceAnchor.up);
            Quaternion rotationOffset = targetRotation * Quaternion.Inverse(entranceRotation);

            endRoom.transform.rotation = rotationOffset * endRoom.transform.rotation;

            Vector3 entranceWorldPosition = endRoom.transform.TransformPoint(entranceAnchor.localPosition);
            Vector3 positionOffset = lastExitAnchor.position - entranceWorldPosition;
            endRoom.transform.position += positionOffset;

            Debug.Log("End room generated and aligned successfully.");
        }
        else
        {
            Debug.LogWarning("EntranceAnchor not found on end room prefab.");
        }
    }

    private void TrySpawnItemsInRoom(GameObject room)
    {
        Transform[] itemAnchors = room.GetComponentsInChildren<Transform>();
        List<Transform> itemAnchorList = new List<Transform>();

        foreach (var anchor in itemAnchors)
        {
            if (anchor.name.Contains("ItemAnchor"))
            {
                itemAnchorList.Add(anchor);
            }
        }

        foreach (Transform itemAnchor in itemAnchorList)
        {
            if (Random.Range(0f, 1f) <= itemSpawnChance)
            {
                GameObject randomItem = itemsToSpawn[Random.Range(0, itemsToSpawn.Count)];
                Instantiate(randomItem, itemAnchor.position, itemAnchor.rotation, room.transform);
            }
        }
    }
}

// This class handles the OpenDoorTrigger logic directly within the script.
public class OpenDoorTrigger : MonoBehaviour
{
    private RoomGen roomGen;
    private int roomIndex;

    public void Initialize(RoomGen gen, int index)
    {
        roomGen = gen;
        roomIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roomGen.OnPlayerEnterRoom(roomIndex);
            Destroy(this.gameObject); // Delete the trigger once activated
        }
    }
}
