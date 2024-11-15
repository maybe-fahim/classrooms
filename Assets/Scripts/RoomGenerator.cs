using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab; // Reference to the start room prefab
    public GameObject endRoomPrefab; // Reference to the end room prefab
    public GameObject[] roomPrefabs; // Array of room prefabs for intermediate rooms
    public int numberOfRooms = 3; // Number of intermediate rooms to generate
    public int seed = 0; // Seed for random generation, set to 0 to use a random seed

    private Vector3 nextRoomPosition; // Position to place the next room
    private Quaternion nextRoomRotation = Quaternion.identity; // Rotation for the next room

    void Start()
    {
        // Set seed for reproducibility
        if (seed == 0)
        {
            seed = Random.Range(1, int.MaxValue);
        }
        Random.InitState(seed);
        Debug.Log("Generation Seed: " + seed);

        // Place the start room
        PlaceRoom(startRoomPrefab, -1); // No previous room for start room

        // Place intermediate rooms
        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject selectedRoom = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            PlaceRoom(selectedRoom, i); // Pass the index to check anchor matches
        }

        // Place the end room separately, like the start room
        PlaceEndRoom(endRoomPrefab);
    }

    void PlaceRoom(GameObject roomPrefab, int index)
    {
        // Instantiate the room at the next position and rotation
        GameObject newRoom = Instantiate(roomPrefab, nextRoomPosition, nextRoomRotation);

        // Find the entrance and exit anchors in the new room
        Transform entranceAnchor = newRoom.transform.Find("EntranceAnchor");
        Transform exitAnchor = newRoom.transform.Find("ExitAnchor");

        if (entranceAnchor != null && exitAnchor != null)
        {
            // If both anchors are found, proceed as usual
            CalculateRoomPositionAndRotation(entranceAnchor, exitAnchor, newRoom);
        }
        else if (entranceAnchor == null && exitAnchor != null)
        {
            // If the room only has an exit anchor, use it for positioning
            CalculateRoomPositionAndRotationForExitAnchorOnly(exitAnchor, newRoom);
        }
        else if (entranceAnchor != null && exitAnchor == null)
        {
            // If the room only has an entrance anchor (e.g., end room), use it for positioning
            CalculateRoomPositionAndRotationForEntranceAnchorOnly(entranceAnchor, newRoom);
        }
        else
        {
            Debug.LogWarning("No valid anchors found in " + roomPrefab.name);
        }
    }

    void CalculateRoomPositionAndRotation(Transform entranceAnchor, Transform exitAnchor, GameObject newRoom)
    {
        // Calculate the rotation to align the entrance of the new room to the exit of the previous room
        Quaternion rotationDifference = Quaternion.FromToRotation(entranceAnchor.forward, nextRoomRotation * Vector3.forward);
        newRoom.transform.rotation = rotationDifference * newRoom.transform.rotation;

        // Reposition the new room to align its entrance with the previous room's exit
        Vector3 offset = entranceAnchor.position - newRoom.transform.position;
        newRoom.transform.position = nextRoomPosition - offset;

        // Update nextRoomPosition and nextRoomRotation for the next room to be placed
        nextRoomPosition = exitAnchor.position;
        nextRoomRotation = exitAnchor.rotation;
    }

    void CalculateRoomPositionAndRotationForExitAnchorOnly(Transform exitAnchor, GameObject newRoom)
    {
        // For rooms that only have an exit anchor (e.g., start room), we just use the exit anchor for the next position and rotation
        Vector3 offset = exitAnchor.position - newRoom.transform.position;
        newRoom.transform.position = nextRoomPosition - offset;
        nextRoomPosition = exitAnchor.position;
        nextRoomRotation = exitAnchor.rotation;
    }

    void CalculateRoomPositionAndRotationForEntranceAnchorOnly(Transform entranceAnchor, GameObject newRoom)
    {
        // For rooms that only have an entrance anchor (e.g., end room), we just use the entrance anchor to set the position
        Vector3 offset = entranceAnchor.position - newRoom.transform.position;
        newRoom.transform.position = nextRoomPosition - offset;
        nextRoomPosition = entranceAnchor.position;
        nextRoomRotation = entranceAnchor.rotation;
    }

    void PlaceEndRoom(GameObject roomPrefab)
    {
        // Instantiate the end room at the next position and rotation
        GameObject newRoom = Instantiate(roomPrefab, nextRoomPosition, nextRoomRotation);

        // Find the entrance anchor in the end room
        Transform entranceAnchor = newRoom.transform.Find("EntranceAnchor");

        if (entranceAnchor != null)
        {
            // Convert the entrance anchor position to world space (global coordinates)
            Vector3 worldPosition = newRoom.transform.TransformPoint(entranceAnchor.localPosition);

            // Debug: log the world position of the entrance anchor
            Debug.Log("World Position of Entrance Anchor: " + worldPosition);

            // Ensure the room faces the correct direction (rotate the room 180 degrees if necessary)
            newRoom.transform.Rotate(0, 180f, 0);

            // Debug: log the value of nextRoomPosition.y before placing the end room
            Debug.Log("Next Room Position Y before placing end room: " + nextRoomPosition.y);

            // Explicitly set the Y position to nextRoomPosition.y to ensure correct alignment
            newRoom.transform.position = new Vector3(worldPosition.x, nextRoomPosition.y, worldPosition.z);

            // Update nextRoomPosition to the end room's position and nextRoomRotation to its entrance anchor's rotation
            nextRoomPosition = newRoom.transform.position;
            nextRoomRotation = entranceAnchor.rotation;

            // Log final placement for debugging
            Debug.Log("Placed end room at position: " + newRoom.transform.position);
        }
        else
        {
            Debug.LogWarning("EntranceAnchor not found in end room prefab");
        }
    }
}
