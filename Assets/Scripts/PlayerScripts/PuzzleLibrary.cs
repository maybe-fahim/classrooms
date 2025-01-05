using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleLibrary : MonoBehaviour
{
    public GameObject book1UI;
    public GameObject book2UI;
    public GameObject book3UI;
    public GameObject book4UI;
    public GameObject book5UI;
    public GameObject pickUpUI;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputActionReference interactionInput;
    [SerializeField] private GameObject reticleUI; // Reference to the Reticle UI
    [SerializeField] private float interactionRange = 5f; // Range for the raycast
    [SerializeField] private GameObject holdPos; // Reference to the hold position

    private List<GameObject> availableBookUIs; // List to track unused BookUI objects
    private bool isUIActive = false; // Tracks if the UI is active

    void Start()
    {
        // Initialize the list of available BookUI objects
        availableBookUIs = new List<GameObject> { book1UI, book2UI, book3UI, book4UI, book5UI };

        // Ensure initial states
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (reticleUI != null) reticleUI.SetActive(true);
        if (pickUpUI != null) pickUpUI.SetActive(false);

        // Ensure there's an EventSystem in the scene
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogWarning("No EventSystem found in the scene. UI buttons will not work!");
        }
    }

    private void OnEnable()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed += HandleInteraction;
        }
    }

    private void OnDisable()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed -= HandleInteraction;
        }
    }

    void FixedUpdate()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.CompareTag("PuzzleBook"))
            {
                if (pickUpUI != null) pickUpUI.SetActive(true);
            }
            else
            {
                if (pickUpUI != null) pickUpUI.SetActive(false);
            }
        }
    }

    private void HandleInteraction(InputAction.CallbackContext context)
    {
        if (playerCamera == null || isUIActive) return;

        // Perform a raycast to check for objects with the "PuzzleBook" tag
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.CompareTag("PuzzleBook"))
            {
                HandlePuzzleBookInteraction(hit.collider.gameObject);
            }
        }
    }

    private void HandlePuzzleBookInteraction(GameObject puzzleBook)
    {
        // Delete the hit game object
        Destroy(puzzleBook);
        holdPos.SetActive(false);


        // Check if there are any available BookUI objects left
        if (availableBookUIs.Count > 0)
        {
            // Choose a random BookUI object and make it active
            int randomIndex = Random.Range(0, availableBookUIs.Count);
            GameObject selectedBookUI = availableBookUIs[randomIndex];
            selectedBookUI.SetActive(true);

            // Remove the selected BookUI from the list
            availableBookUIs.RemoveAt(randomIndex);

            Debug.Log($"Activated {selectedBookUI.name}");

            // Hide the Reticle UI and unlock the cursor
            if (reticleUI != null) reticleUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isUIActive = true; // Mark the UI as active
        }
        else
        {
            Debug.Log("No more BookUI objects available.");
        }
    }

    public void CloseBookUI()
    {
        // Reset all BookUI objects to inactive
        foreach (var bookUI in new[] { book1UI, book2UI, book3UI, book4UI, book5UI })
        {
            if (bookUI != null) bookUI.SetActive(false);
        }

        // Re-enable the Reticle UI and lock the cursor
        if (reticleUI != null) reticleUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        holdPos.SetActive(true);

        isUIActive = false; // Mark the UI as inactive

        Debug.Log("UI closed and reverted to normal state.");
    }
}
