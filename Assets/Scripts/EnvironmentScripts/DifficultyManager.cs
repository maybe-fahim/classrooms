using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float maxTime;
    public float extraTimePerRoom;

    public float startTime;

    [Header("Player Settings")]
    public float playerMaxHP;

    [Header("Item Spawn Settings")]
    public float itemSpawnRate;

    [Header("Light Settings")]
    public float chanceLightOn;
    public float chanceLightFlicker;
    public float chanceLightOff;

    [Header("Enemy Camera Settings")]
    public float enemyCameraSpawnRate;

    [Header("Room Generation Settings")]
    public int numberOfIntermediateRooms;


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
        // Load the difficulty from PlayerPrefs and set values
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0); // Default to Easy

        switch (difficulty)
        {
            case 0: // Easy
                SetEasyDifficulty();
                break;
            case 1: // Medium
                SetMediumDifficulty();
                break;
            case 2: // Hard
                SetHardDifficulty();
                break;
            default:
                Debug.LogWarning("Unknown difficulty level! Defaulting to Easy.");
                SetEasyDifficulty();
                break;
        }

        // Apply room settings
        ApplyRoomSettings();
    }

    private void SetEasyDifficulty()
    {
        maxTime = 300f;
        extraTimePerRoom = 60f;
        playerMaxHP = 150f;
        itemSpawnRate = 0.8f;
        chanceLightOn = 0.7f;
        chanceLightFlicker = 0.3f;
        chanceLightOff = 0f;
        enemyCameraSpawnRate = 0.2f;
        numberOfIntermediateRooms = 100;

        Debug.Log("Easy difficulty applied.");
    }

    private void SetMediumDifficulty()
    {
        maxTime = 200f;
        extraTimePerRoom = 40f;
        playerMaxHP = 100f;
        itemSpawnRate = 0.6f;
        chanceLightOn = 0.2f;
        chanceLightFlicker = 0.6f;
        chanceLightOff = 0.2f;
        enemyCameraSpawnRate = 0.4f;
        numberOfIntermediateRooms = 100;

        Debug.Log("Medium difficulty applied.");
    }

    private void SetHardDifficulty()
    {
        maxTime = 180f;
        extraTimePerRoom = 30f;
        playerMaxHP = 80f;
        itemSpawnRate = 0.5f;
        chanceLightOn = 0f;
        chanceLightFlicker = 0.6f;
        chanceLightOff = 0.4f;
        enemyCameraSpawnRate = 0.6f;
        numberOfIntermediateRooms = 120;

        Debug.Log("Hard difficulty applied.");
    }

    private void ApplyRoomSettings()
    {
        if (roomGen != null)
        {
            roomGen.SetNumberOfIntermediateRooms(numberOfIntermediateRooms);
        }
        else
        {
            Debug.LogWarning("RoomGen reference not set in DifficultyManager.");
        }
    }
}
