using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    // References for animators and MeshColliders
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Animator myHandle = null;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource doorAudioSource = null; 
    [SerializeField] private AudioClip doorOpenClip = null;      

    private RoomCounter roomCounter; 
    private TimeKeeper timeKeeper;
    private DifficultyManager difficultyManager; // We'll pull extraTimePerRoom from here

    void Start()
    {
        // 1) Find the RoomCounter in the scene
        roomCounter = FindObjectOfType<RoomCounter>();
        if (roomCounter == null)
        {
            Debug.LogError("RoomCounter not found in the scene.");
        }

        // 2) Find the TimeKeeper in the scene
        timeKeeper = FindObjectOfType<TimeKeeper>();
        if (timeKeeper == null)
        {
            Debug.LogError("TimeKeeper not found in the scene.");
        }

        // 3) Find DifficultyManager for extraTimePerRoom
        difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager == null)
        {
            Debug.LogError("DifficultyManager not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Play animations
            if (myDoor != null)
            {
                myDoor.Play("openDoor", 0, 0.0f);
            }
            if (myHandle != null)
            {
                myHandle.Play("openDoorHandle", 0, 0.0f);
            }

            // Only increment time if this is an EntranceDoor
            if (IsEntranceDoorTrigger())
            {
                // Increment the room count
                if (roomCounter != null)
                {
                    roomCounter.roomCount++;
                    Debug.Log("Room count incremented. Current count: " + roomCounter.roomCount);
                }

                // Add extra time from DifficultyManager
                if (timeKeeper != null && difficultyManager != null)
                {
                    float extraTime = difficultyManager.GetExtraTimePerRoom();
                    timeKeeper.AddTime(extraTime); 
                    Debug.Log($"Added {extraTime} seconds to the timer.");
                }
            }

            // Play door open sound
            PlayDoorOpenSound();

            // Disable the BoxCollider of the OpenDoorTrigger
            BoxCollider triggerCollider = GetComponent<BoxCollider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            // Disable the MeshCollider of the targetObject
            if (targetObject != null)
            {
                MeshCollider meshCollider = targetObject.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.enabled = false;
                }
            }
        }
    }

    private void PlayDoorOpenSound()
    {
        if (doorAudioSource != null && doorOpenClip != null)
        {
            doorAudioSource.PlayOneShot(doorOpenClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or DoorOpenClip is not assigned.");
        }
    }

    private bool IsEntranceDoorTrigger()
    {
        Transform parent = transform.parent;
        if (parent != null && parent.gameObject.name.Contains("EntranceDoor"))
        {
            return true;
        }
        return false;
    }
}
