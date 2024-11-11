using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField]
    private Hotbar hotbar;

    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[5];
    [SerializeField] Image[] inventorySlotBackground = new Image[5];
    [SerializeField] Sprite emptySlotSprite;

    [SerializeField] GameObject throwItem_gameobject;


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
            if (hit.collider.GetComponent<ItemPickable>())
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
        IPickable item = hit.collider.GetComponent<IPickable>();
        if (item != null)
        {
            itemType itemTypeToAdd = hit.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type;
            
            // Add the item type to the hotbar's inventory list
            hotbar.inventoryList.Add(itemTypeToAdd);
            item.PickItem();

            
            
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
        if (hotbar.inventoryList.Count > 0)
        {
            // Determine the spawn point in front of the player
            Vector3 spawnPosition = playerCameraTransform.position + playerCameraTransform.forward * 2f; // Adjust 2f as needed

            // Cast a ray downwards from the spawn position to snap to the ground
            RaycastHit groundHit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 2f, Vector3.down, out groundHit, 5f, pickablelayerMask))
            {
                spawnPosition = groundHit.point; // Adjust spawn position to the ground level
            }

            Instantiate(hotbar.itemInstantiate[hotbar.inventoryList[hotbar.selectedItem]], position: spawnPosition, new Quaternion());
            hotbar.inventoryList.RemoveAt(hotbar.selectedItem);
            
            if(hotbar.selectedItem != 0)
            {
                hotbar.selectedItem-= 1;
            }
            hotbar.NewItemSelected();
        }
        


        
        /*
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
        */
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

        for (int i = 0; i < 5; i++)
        {
            if(i < hotbar.inventoryList.Count)
            {
                Sprite itemSprite = hotbar.itemSetActive[hotbar.inventoryList[i]].GetComponent<ItemPickable>().itemScriptableObject.item_sprite;
                inventorySlotImage[i].sprite = itemSprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
                
        }
        
        int a = 0;
        foreach (Image image in inventorySlotBackground)
        {
            if(a == hotbar.selectedItem)
            {
                image.color = Color.yellow;
            }
            else
            {
                image.color = Color.white;
            }
            a++;
        }

        
  }

    private void HighlightObject()
    {
        // Highlight the object that the raycast is hitting
        hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);

        // If the hit object is an item, show pickup UI
        if (hit.collider.GetComponent<ItemPickable>() || hit.collider.GetComponent<LeverDoor>())
        {
            pickUpUI.SetActive(true);
        }
    }
}

public interface IPickable
{
    void PickItem();
}