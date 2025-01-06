using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject endRoomPrefab;
    public GameObject boss1RoomPrefab; // Boss 1 Room prefab
    public GameObject shopRoomPrefab; // Shop Room prefab
    public List<GameObject> intermediateRoomPrefabs;
    public List<GameObject> intermediateTurnRoomPrefabs;
    public int numberOfIntermediateRooms = 5;
    public int randomSeed = 0; // Default seed is 0, meaning it will be randomized

    public GameObject homeworkPrefab; // Prefab for the homework item

    private List<GameObject> generatedRooms = new List<GameObject>();
    private int currentRoomIndex = 0; // Tracks currently generated room index
    private int lastTurnRoomIndex = -4; // Ensure the first turn room can be generated
    private bool boss1RoomGenerated = false; // Tracks if Boss 1 Room has been placed
    private bool shopRoomGenerated = false; // Tracks if Shop Room has been placed
    private int bossRoomPosition; // The calculated position of the Boss 1 Room
    private int totalIntermediateRoomsGenerated = 0; // Tracks all intermediate rooms, including initial ones

    private void Start()
    {
        // Calculate the Boss 1 Room position (1-based index), accounting for odd/even cases
        bossRoomPosition = Mathf.FloorToInt(numberOfIntermediateRooms / 2f);

        GenerateInitialRooms();
        SpawnPlayer();
    }

    public void SetNumberOfIntermediateRooms(int newCount)
    {
        numberOfIntermediateRooms = newCount;
        // If you need to regenerate rooms dynamically, you could handle that here.
        // But for now, it just updates the field before Start() runs or at runtime.
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

        // Generate the first 3 rooms
        for (int i = 0; i < 3; i++)
        {
            GameObject nextRoom = GenerateRoom(lastExitAnchor);
            lastExitAnchor = nextRoom.transform.Find("ExitAnchor");
        }
    }

    private GameObject GenerateRoom(Transform lastExitAnchor)
    {
        GameObject nextRoomPrefab;

        // Check if the Boss 1 Room should be placed
        if (!boss1RoomGenerated && totalIntermediateRoomsGenerated == bossRoomPosition)
        {
            nextRoomPrefab = boss1RoomPrefab;
            boss1RoomGenerated = true;
        }
        else if (boss1RoomGenerated && !shopRoomGenerated)
        {
            // Place the shop room immediately after the boss 1 room
            nextRoomPrefab = shopRoomPrefab;
            shopRoomGenerated = true;
        }
        else
        {
            // Determine if a turn room should be generated
            if (currentRoomIndex - lastTurnRoomIndex >= 3 && intermediateTurnRoomPrefabs.Count > 0)
            {
                float turnRoomChance = 0.5f;
                if (Random.Range(0f, 1f) < turnRoomChance)
                {
                    nextRoomPrefab = intermediateTurnRoomPrefabs[Random.Range(0, intermediateTurnRoomPrefabs.Count)];
                    lastTurnRoomIndex = currentRoomIndex;
                }
                else
                {
                    nextRoomPrefab = intermediateRoomPrefabs[Random.Range(0, intermediateRoomPrefabs.Count)];
                }
            }
            else
            {
                nextRoomPrefab = intermediateRoomPrefabs[Random.Range(0, intermediateRoomPrefabs.Count)];
            }
        }

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
            Transform doorTrigger = nextRoom.transform.Find("EntranceDoor/OpenDoorTrigger");
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

        // Check if the room index ends with 5 and spawn the homework item
        if (currentRoomIndex % 10 == 5)
        {
            Transform homeworkAnchor = nextRoom.transform.Find("HomeworkAnchor");
            if (homeworkAnchor != null && homeworkPrefab != null)
            {
                Instantiate(homeworkPrefab, homeworkAnchor.position, homeworkAnchor.rotation, homeworkAnchor);
                Debug.Log($"Homework spawned in room index {currentRoomIndex}.");
            }
            else
            {
                Debug.LogWarning($"HomeworkAnchor not found or HomeworkPrefab is null in room index {currentRoomIndex}.");
            }
        }

        totalIntermediateRoomsGenerated++; // Increment the intermediate room count
        currentRoomIndex++;
        return nextRoom;
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && generatedRooms.Count > 0)
        {
            GameObject startRoom = generatedRooms[0];
            Transform playerSpawnPoint = startRoom.transform.Find("PlayerSpawnPoint");

            if (playerSpawnPoint != null)
            {
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
            Debug.LogWarning("Player object not found in the scene or no rooms generated.");
        }
    }

    public void OnPlayerEnterRoom(int roomIndex)
    {
        currentRoomIndex = roomIndex;

        // Enable DoorBarrier in the room five behind
        int barrierRoomIndex = roomIndex - 3; // Room whose barrier should be enabled
        if (barrierRoomIndex >= 0 && barrierRoomIndex < generatedRooms.Count)
        {
            GameObject barrierRoom = generatedRooms[barrierRoomIndex];
            if (barrierRoom != null)
            {
                Transform doorBarrier = barrierRoom.transform.Find("DoorBarrier");
                if (doorBarrier != null)
                {
                    MeshRenderer meshRenderer = doorBarrier.GetComponent<MeshRenderer>();
                    MeshCollider meshCollider = doorBarrier.GetComponent<MeshCollider>();
                    if (meshRenderer != null) meshRenderer.enabled = true;
                    if (meshCollider != null) meshCollider.enabled = true;
                }
            }
        }

        // Generate the next intermediate room if applicable
        if (roomIndex + 3 < numberOfIntermediateRooms + 1 && roomIndex + 3 >= generatedRooms.Count)
        {
            Transform lastExitAnchor = generatedRooms[generatedRooms.Count - 1].transform.Find("ExitAnchor");
            GenerateRoom(lastExitAnchor);
        }

        // Generate the end room when approaching the final room
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
            Quaternion targetRotation = Quaternion.LookRotation(-lastExitAnchor.forward, lastExitAnchor.up);
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
        }
    }
}
