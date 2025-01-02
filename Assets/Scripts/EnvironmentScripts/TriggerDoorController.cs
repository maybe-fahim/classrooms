using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerDoorController : MonoBehaviour
{
    // references for animators and mesh coliders
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Animator myHandle = null;
    private RoomCounter roomCounter; // Reference to the RoomCounter script
    private TimeKeeper timeKeeper; // Reference to the TimeKeeper script

    void Start()
    {
        // Find the RoomCounter in the scene
        roomCounter = FindObjectOfType<RoomCounter>();
        if (roomCounter == null)
        {
            Debug.LogError("RoomCounter not found in the scene.");
        }

        // Find the TimeKeeper in the scene
        timeKeeper = FindObjectOfType<TimeKeeper>();
        if (timeKeeper == null)
        {
            Debug.LogError("TimeKeeper not found in the scene.");
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Play the door opening animation
            myDoor.Play("openDoor", 0, 0.0f);

            // Play the handle turning animation
            myHandle.Play("openDoorHandle", 0, 0.0f);

             // Increment the room count
            if (roomCounter != null)
            {
                roomCounter.roomCount = roomCounter.roomCount + 1;
                Debug.Log("Room count incremented. Current count: " + roomCounter.roomCount);
            }
            else
            {
                Debug.LogError("RoomCounter is not assigned.");
            }

            if (timeKeeper != null)
            {
                timeKeeper.time = timeKeeper.time + 60; // Add 60 seconds to the timer
            }

            // Disable the open door trigger
            gameObject.SetActive(false);

            // disable its MeshCollider
            if (targetObject != null)
            {
                targetObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}
