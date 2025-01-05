using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PCGLockedDoor : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5f; // Distance for the raycast
    [SerializeField] private LayerMask doorLayer; // LayerMask to ensure only the door is hit
    [SerializeField] private InputActionReference interactionInput;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Camera playerCam; // Reference to the player's camera

    public GameObject noKeyUI;
    public GameObject useKeyUI;
    private bool playerHasKey = false;
    private RoomCounter roomCounter; // Reference to the RoomCounter script
    private TimeKeeper timeKeeper; // Reference to the TimeKeeper script
    public bool isLocked; // Determines if the door is locked
    public GameObject trigger; // Reference to the trigger GameObject
    public GameObject keys; // Parent object containing child key objects
    public float lockedChance; // Chance for the door to be locked

    private void Start()
    {
        // Randomly decide if the door is locked
        isLocked = Random.value < lockedChance; //chance to be locked

        if (isLocked)
        {
            Debug.Log("The door is locked.");
            // Disable the trigger
            if (trigger != null)
            {
                trigger.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Trigger is not assigned.");
            }

            // Activate a random key
            if (keys != null)
            {
                ActivateRandomKey();
            }
            else
            {
                Debug.LogWarning("Keys object is not assigned.");
            }
        }
        else
        {
            Debug.Log("The door is unlocked.");
            // Leave the trigger active
        }

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

        if (playerCam == null)
        {
            Debug.LogError("Player camera is not assigned. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (!isLocked)
        {
            return;
        }
        // Check if the key is in the hand
        if (key != null)
        {
            playerHasKey = key.activeSelf;
        }
         // Perform raycast
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition); // Or use the player's camera direction
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, doorLayer))
        {
            // Check if the raycast hits this door object
            if (hit.collider.gameObject == targetObject)
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
        if (!isLocked)
        {
            return;
        }
        // Perform raycast
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition); // Or use the player's camera direction
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, doorLayer))
        {
            // Check if the raycast hits this door object
            if (hit.collider.gameObject == targetObject)
            {
                if (playerHasKey)
                {
                    trigger.SetActive(true);
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
                else
                {
                    Debug.Log("You need a key to open this door.");
                }
            }
        }
    }

    private void ActivateRandomKey()
    {
        int keyCount = keys.transform.childCount;
        if (keyCount > 0)
        {
            // Deactivate all keys initially
            foreach (Transform child in keys.transform)
            {
                child.gameObject.SetActive(false);
            }

            // Activate a random key
            int randomIndex = Random.Range(0, keyCount);
            Transform randomKey = keys.transform.GetChild(randomIndex);
            randomKey.gameObject.SetActive(true);

            Debug.Log($"Key {randomKey.name} is now active.");
        }
        else
        {
            Debug.LogWarning("No keys found in the keys object.");
        }
    }


}
