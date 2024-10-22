using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickablelayerMask;
    [SerializeField]
    private Transform playerCameraTransform;
    [SerializeField]
    private GameObject pickUpUI;
    [SerializeField]
    private GameObject interactUI;
    [SerializeField]
    [Min(1)]
    private float hitRange = 5f;

    [SerializeField]
    private InputActionReference interactionInput, dropInput;
    
    private RaycastHit hit;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private GameObject inHandItem;
    private Vector3 originalScale;

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        // If the player is already holding an item, do nothing
        if (inHandItem != null)
        {
            return;
        }

        if (hit.collider != null)
        {
            // Check if player hit a pickable item
            if (hit.collider.GetComponent<Item>())
            {
                PickUpItem();
            }
            // Check if player hit a lever
            else if (hit.collider.GetComponent<LeverDoor>())
            {
                InteractWithLever();
            }
        }
    }

    private void PickUpItem()
    {
        Debug.Log("Picked up " + hit.collider.name);
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        inHandItem = hit.collider.gameObject;
        inHandItem.transform.position = Vector3.zero;
        inHandItem.transform.rotation = Quaternion.identity;
        originalScale = inHandItem.transform.localScale;
        inHandItem.transform.SetParent(pickUpParent.transform, false);

        if (rb != null)
        {
            rb.isKinematic = true; // Make sure the item doesn't move after picking up
        }
    }

    private void InteractWithLever()
    {
        LeverDoor lever = hit.collider.GetComponent<LeverDoor>();

        if (lever != null)
        {
            Debug.Log("Lever found! Checking state...");

            // Check if the lever has already been flicked
            if (!lever.IsLeverFlicked())
            {
                Debug.Log("Lever is not flicked yet. Flicking it now...");
                lever.FlickLever();
            }
            else
            {
                Debug.Log("This lever has already been flicked.");
            }
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            inHandItem.transform.localScale = originalScale;
            inHandItem = null;
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.isKinematic = false; // Re-enable physics when dropped
            }
        }
    }

    private void Update()
    {
        // If an object was previously highlighted, remove the highlight
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
            interactUI.SetActive(false);
        }

        // Don't cast ray when holding an item
        if (inHandItem != null)
        {
            return;
        }

        // Raycast from camera to detect objects within range
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickablelayerMask))
        {
            if(hit.collider.GetComponent<LeverDoor>())
            {
                LeverDoor lever = hit.collider.GetComponent<LeverDoor>();
                if(lever.IsLeverFlicked())
                {
                    interactUI.SetActive(false);
                } 
                else
                {
                    interactUI.SetActive(true);
                }
            }
            else
            {
                HighlightObject();
            }
            
        }
    }

    private void HighlightObject()
    {
        // Highlight the object that the raycast is hitting
        hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);

        // If the hit object is an item, show pickup UI
        if (hit.collider.GetComponent<Item>() || hit.collider.GetComponent<LeverDoor>())
        {
            pickUpUI.SetActive(true);
        }
    }
}
