using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    private Image cursor;
    [SerializeField]
    private Sprite hand;
    [SerializeField]
    private Sprite normal;
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
    private int homeworkCount = 0;
    private int currency = 0;

    [SerializeField]
    private Hotbar hotbar;

    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[5];
    [SerializeField] Image[] inventorySlotBackground = new Image[5];
    [SerializeField] Sprite emptySlotSprite;
    public TextMeshProUGUI homeworkPopup; // Reference to the TextMeshProUGUI element 

    public TextMeshProUGUI PaperClipsText; // Reference to the TextMeshProUGUI component

    [SerializeField] GameObject throwItem_gameobject;


    private void Start()
    {
        interactionInput.action.performed += Interact;
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
                if (hotbar.inventoryList.Count < 5)
                {
                    PickUpItem();
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
            // Check if player hit a lever
            else if (hit.collider.GetComponent<LeverDoor>())
            {
                InteractWithLever();
            }
            else if (hit.collider.name.Contains("Homework"))
            {
                SecretHomework();
            }
            else if (hit.collider.name.Contains("Clip"))
            {
                CurrencyManager();
            }
        }
    }

    private void PickUpItem()
    {
        Debug.Log("Picked up " + hit.collider.name);
        IPickable item = hit.collider.GetComponent<IPickable>();
        if (item != null)
        {
            pickUpUI.SetActive(false);
            cursor.sprite = normal;
            itemType itemTypeToAdd = hit.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type;

            // Check if the item is a flashlight
            if (itemTypeToAdd == itemType.Torch)
            {
                // Check if the inventory already contains a flashlight
                if (hotbar.inventoryList.Contains(itemType.Torch))
                {
                    Debug.Log("Flashlight already in inventory. Refilling battery life.");

                    // Find the flashlight in the scene and refill its battery
                    Flashlight flashlight = FindObjectOfType<Flashlight>();
                    if (flashlight != null)
                    {
                        flashlight.batteryLife = 100; // Refill the battery
                    }

                    // Destroy the new flashlight being picked up since we don't add it to the inventory
                    Destroy(hit.collider.gameObject);
                    return;
                }
            }

            // Add the item type to the hotbar's inventory list
            hotbar.inventoryList.Add(itemTypeToAdd);
            item.PickItem();
        }
    }

    private void SecretHomework()
    {
    
        Debug.Log("Picked up " + hit.collider.name);
        pickUpUI.SetActive(false);
        cursor.sprite = normal;
        homeworkCount++;
        

        // Destroy the new flashlight being picked up since we don't add it to the inventory
        Destroy(hit.collider.gameObject);

        // Show the homework count in the UI
        StartCoroutine(ShowHomeworkPopup());

        return;
        
    }
    
    private void CurrencyManager()
    {
    Debug.Log("You picked up some clips!");
    pickUpUI.SetActive(false);
    cursor.sprite = normal;

    // Check the parent of the hit object
    Transform parent = hit.collider.transform.parent;
    if (parent != null) // Ensure the object has a parent
    {
            // Check the tag of the parent object
            if (parent.CompareTag("LargeClip"))
            {
                currency += 100; // Add 100 for a large clip
                Debug.Log("Large clip picked up! Currency: " + currency);
            }
            else if (parent.CompareTag("MediumClip"))
            {
                currency += 50; // Add 50 for a medium clip
                Debug.Log("Medium clip picked up! Currency: " + currency);
            }
            else if (parent.CompareTag("SmallClip"))
            {
                currency += 25; // Add 25 for a small clip
                Debug.Log("Small clip picked up! Currency: " + currency);
            }
            else
            {
                Debug.Log("Parent does not have a recognized tag.");
            }
        }
        else
        {
            Debug.Log("Hit object has no parent.");
        }

        // Update the TextMeshPro UI text
        UpdatePaperClipsText();

        // Destroy the parent object
        Destroy(parent.gameObject);
    }

    private void UpdatePaperClipsText()
    {
        if (PaperClipsText != null)
        {
            PaperClipsText.text = $"Paper Clips: {currency}";
        }
        else
        {
            Debug.LogWarning("PaperClipsText is not assigned in the Inspector.");
        }
    }

    private IEnumerator ShowHomeworkPopup()
    {
        if (homeworkCount > 9)
        {
            homeworkPopup.text = $"All the homework has been collected!";
            homeworkPopup.gameObject.SetActive(true); // Enable the text
            Debug.Log("Homework count: " + homeworkCount);
            yield return new WaitForSeconds(10f); // Wait for 10 seconds
            homeworkPopup.gameObject.SetActive(false); // Disable the text
        }
        else
        {
            homeworkPopup.text = $"Homework: {homeworkCount}/9";
            homeworkPopup.gameObject.SetActive(true); // Enable the text
            Debug.Log("Homework count: " + homeworkCount);
            yield return new WaitForSeconds(4f); // Wait for 10 seconds
            homeworkPopup.gameObject.SetActive(false); // Disable the text
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


    private void FixedUpdate()
    {
        // If an object was previously highlighted, remove the highlight
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
            interactUI.SetActive(false);
            cursor.sprite = normal;
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
        if (hit.collider.GetComponent<ItemPickable>() || hit.collider.GetComponent<LeverDoor>()|| hit.collider.name.Contains("Homework")|| hit.collider.name.Contains("ClipPile"))
        {
            if ((hotbar.inventoryList.Count < 5) || hit.collider.name.Contains("Homework")|| hit.collider.name.Contains("ClipPile"))
            {
                
                cursor.sprite = hand;
                pickUpUI.SetActive(true);
            }
            
        }
    }
}

public interface IPickable
{
    void PickItem();
}