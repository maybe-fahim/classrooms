using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Starting time for the level/game in seconds")]
    [SerializeField] private float startTime = 300f; // Example default: 5 minutes

    [Tooltip("Maximum possible time in seconds (never exceed this)")]
    [SerializeField] private float maxTime = 600f;   // Example default: 10 minutes

    [Tooltip("Extra time added each time the player enters a new room")]
    [SerializeField] private float extraTimePerRoom = 10f;


    [Header("Player Settings")]
    [Tooltip("Player's maximum HP")]
    [SerializeField] private float playerMaxHP = 100f;


    [Header("Item Spawn Settings")]
    [Range(0f, 1f), Tooltip("Probability or rate at which items spawn")]
    [SerializeField] private float itemSpawnRate = 0.2f;


    [Header("Light Settings")]
    [Range(0f, 1f), Tooltip("Probability that a light is ON")]
    [SerializeField] private float chanceLightOn = 0.6f;

    [Range(0f, 1f), Tooltip("Probability that a light is FLICKERING")]
    [SerializeField] private float chanceLightFlicker = 0.3f;

    [Range(0f, 1f), Tooltip("Probability that a light is OFF")]
    [SerializeField] private float chanceLightOff = 0.1f;


    [Header("Enemy Camera Settings")]
    [Range(0f, 1f), Tooltip("Probability or rate at which enemy cameras are spawned")]
    [SerializeField] private float enemyCameraSpawnRate = 0.15f;


    [Header("Room Generation Settings")]
    [Tooltip("Number of intermediate rooms generated in the level")]
    [SerializeField] private int numberOfIntermediateRooms = 3;

    // ====================================
    // 2. A REFERENCE TO YOUR RoomGen SCRIPT
    // ====================================
    [Header("References")]
    [Tooltip("Assign your 'RoomManager' GameObject (which has RoomGen) here.")]
    [SerializeField] private RoomGen roomGen;


    // ===================
    // Getters (Optional)
    // ===================
    public float GetStartTime() => startTime;
    public float GetMaxTime() => maxTime;
    public float GetExtraTimePerRoom() => extraTimePerRoom;
    public float GetPlayerMaxHP() => playerMaxHP;
    public float GetItemSpawnRate() => itemSpawnRate;
    public float GetChanceLightOn() => chanceLightOn;
    public float GetChanceLightFlicker() => chanceLightFlicker;
    public float GetChanceLightOff() => chanceLightOff;
    public float GetEnemyCameraSpawnRate() => enemyCameraSpawnRate;
    public int GetNumberOfIntermediateRooms() => numberOfIntermediateRooms;

    private void Start()
    {
        // Just an example of setting the number of intermediate rooms
        // at the start, based on our serialized field:
        if (roomGen != null)
        {
            // We’ll call a method in RoomGen that sets the number
            roomGen.SetNumberOfIntermediateRooms(numberOfIntermediateRooms);
        }
        else
        {
            Debug.LogWarning("No RoomGen reference assigned in DifficultyManager.");
        }
    }
}
