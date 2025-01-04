using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyDoor : MonoBehaviour
{
     // references for animators and mesh coliders
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Animator myHandle = null;
    [SerializeField] private GameObject key;
    [SerializeField] private float raycastDistance = 5f; // Distance for the raycast
    [SerializeField] private LayerMask doorLayer; // LayerMask to ensure only the door is hit
    [SerializeField] private InputActionReference interactionInput;
    public GameObject noKeyUI;
    public GameObject useKeyUI;
    private bool playerHasKey = false;
    private RoomCounter roomCounter; // Reference to the RoomCounter script
    private TimeKeeper timeKeeper; // Reference to the TimeKeeper script
    

    private void Start()
    {
        interactionInput.action.performed += Interact;

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

    void Update()
    {
        // Check if the key is in the hand
        if (key != null)
        {
            playerHasKey = key.activeSelf;
        }
         // Perform raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Or use the player's camera direction
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, doorLayer))
        {
            // Check if the raycast hits this door object
            if (hit.collider.gameObject == gameObject)
            {
                if (!playerHasKey)
                {
                    // Show the "no key" UI if the player doesn't have the key
                    noKeyUI.SetActive(true);
                    useKeyUI.SetActive(false);
                }
                else
                {
                    noKeyUI.SetActive(false); // Hide the UI if the key is present
                    useKeyUI.SetActive(true); // Show the "use key" UI
                }
            }
        }
        else
        {
            noKeyUI.SetActive(false); // Hide the UI if the player is not looking at the door
            useKeyUI.SetActive(false); // Hide the "use key" UI
        }
    }



    private void Interact(InputAction.CallbackContext context)
    {
        // Perform raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Or use the player's camera direction
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, doorLayer))
        {
            // Check if the raycast hits this door object
            if (hit.collider.gameObject == gameObject)
            {
                if (playerHasKey)
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("You need a key to open this door.");
                }
            }
        }
    }

    private void OpenDoor()
    {
        // Play the door opening animation
        if (myDoor != null)
        {
            myDoor.Play("openDoor", 0, 0.0f);
        }

        // Play the handle turning animation
        if (myHandle != null)
        {
            myHandle.Play("openDoorHandle", 0, 0.0f);
        }

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

        // -- commented out for now

        //if (timeKeeper != null)
        //{
        //    timeKeeper.time = timeKeeper.time + 60; // Add 60 seconds to the timer
        //}


        // Disable the MeshCollider on the target object
        if (targetObject != null)
        {
            MeshCollider collider = targetObject.GetComponent<MeshCollider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        // Remove the key from the hotbar
        Hotbar hotbar = FindObjectOfType<Hotbar>(); // Ensure the hotbar is in the scene
        if (hotbar != null)
        {
            hotbar.RemoveItemFromHotbar(itemType.Key);
        }
        else
        {
            Debug.LogError("Hotbar not found in the scene.");
        }

    }

}
