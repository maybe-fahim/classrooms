using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Drink : MonoBehaviour
{
    [SerializeField] private InputActionReference useInput;
    public Camera camera;
    public PlayerMovement playerMovement;
    private float drinkLife = 20f;
    public Image drinkUI;
    public GameObject drinkUIObject;
    private bool isDrinking = false;

    private float originalAcceleration;
    private float originalFOV;

    private Hotbar hotbar; // Reference to the hotbar

    // Start is called before the first frame update
    void Start()
    {
        useInput.action.performed += Use;

        // Find the Hotbar instance in the scene
        hotbar = FindObjectOfType<Hotbar>();
        if (hotbar == null)
        {
            Debug.LogError("Hotbar not found in the scene.");
        }
    }

    // Coroutine to handle drinking effect
    private IEnumerator DrinkEffect()
    {
        if (hotbar != null)
        {
            hotbar.LockHotbarSlot();
        }

        // Store the original values
        originalAcceleration = playerMovement.acceleration;
        originalFOV = camera.fieldOfView;

        // Set new values
        playerMovement.acceleration = 500f;
        camera.fieldOfView = 120f;
        drinkUIObject.SetActive(true);

        while (drinkLife > 0)
        {
            drinkLife -= Time.deltaTime; // Decrease the drink life
            drinkUI.fillAmount = drinkLife / 20f; // Update the UI
            yield return null; // Wait for the next frame
        }

        ResetBoost(); // Reset boost when the effect ends
    }

    void Use(InputAction.CallbackContext obj)
    {
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the GameObject.");
            return;
        }

        if (!isDrinking)
        {
            isDrinking = true;

            // Lock the player to the current hotbar slot

            // Start the boost effect
            StartCoroutine(DrinkEffect());
        }
    }

    private void ResetBoost()
    {
        // Reset player properties
        playerMovement.acceleration = originalAcceleration;
        camera.fieldOfView = originalFOV;

        // Deactivate UI
        drinkUIObject.SetActive(false);

        // Unlock the hotbar slot
        if (hotbar != null)
        {
            hotbar.UnlockHotbarSlot();
        }

        // Remove the drink item from the hotbar
        if (hotbar != null)
        {
            hotbar.RemoveItemFromHotbar(itemType.Drink); // Ensure itemType.Drink exists
        }

        isDrinking = false; // Mark as not drinking
        drinkLife = 20f; // Reset drink life for future use
    }

    private void OnDisable()
    {
        if (isDrinking)
        {
            // If the effect is still active when the object is disabled, reset the boost
            ResetBoost();
        }
    }
}
