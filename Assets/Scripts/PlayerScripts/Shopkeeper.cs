using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shopkeeper : MonoBehaviour
{
    [Header("Input Settings")]
    public InputActionReference interactionInput; // Reference to the interact action

    [Header("Cameras")]
    public Camera playerCam; // Player's camera

    [Header("UI Elements")]
    public GameObject Reticle; // Reticle UI
    public GameObject InteractUI; // Interact UI
    public GameObject holdPos; // Hold position UI
    public GameObject hotbarUI; // Hotbar UI
    public GameObject nextDialogueUI; // Next dialogue button UI
    public List<GameObject> dialogueUI; // List of dialogue UI elements
    public GameObject ExitUi;

    [Header("Interaction Settings")]
    public float interactionRange = 5f; // Range for detecting the shop

    private int dialogueIndex = 0; // Tracks current dialogue UI index
    private bool isInteracting = false; // Tracks if interaction is active

    // Start is called before the first frame update
    void Start()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed += OnInteractionPerformed; // Bind interaction action
        }

        // Ensure the cursor is locked and invisible at the start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.CompareTag("Shop"))
            {
                if (!isInteracting && InteractUI != null) InteractUI.SetActive(true);
            }
            else
            {
                if (InteractUI != null) InteractUI.SetActive(false);
            }
        }
        else
        {
            if (InteractUI != null) InteractUI.SetActive(false);
        }
    }

    void Update()
    {
        // Interaction logic is handled in the OnInteractionPerformed method
    }

    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        if (isInteracting) return; // Prevent multiple interactions during dialogue

        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange) && hit.collider.CompareTag("Shop"))
        {
            StartInteraction();
        }
    }

    private void StartInteraction()
    {
        isInteracting = true;

        // Disable reticle, holdPos, hotbarUI, and InteractUI
        if (Reticle != null) Reticle.SetActive(false);
        if (holdPos != null) holdPos.SetActive(false);
        if (hotbarUI != null) hotbarUI.SetActive(false);
        if (InteractUI != null) InteractUI.SetActive(false);


        // Unlock and make cursor visible for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Enable the next dialogue button UI
        if (nextDialogueUI != null) nextDialogueUI.SetActive(true);
        if (ExitUi != null) ExitUi.SetActive(true);

        // Start showing dialogue UI
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        if (dialogueIndex < dialogueUI.Count)
        {
            // Enable the current dialogue UI element
            dialogueUI[dialogueIndex].SetActive(true);
        }
        else
        {
            // End of dialogue, show shop UI
            EndDialogue();
        }
    }

    public void NextDialogue()
    {
        if (dialogueIndex < dialogueUI.Count)
        {
            // Disable the current dialogue UI
            dialogueUI[dialogueIndex].SetActive(false);

            // Move to the next dialogue UI
            dialogueIndex++;

            // Show the next dialogue or end dialogue if finished
            ShowDialogue();
        }
    }

    private void EndDialogue()
    {
        // Ensure all dialogue UI is disabled
        foreach (var ui in dialogueUI)
        {
            ui.SetActive(false);
        }

        // Reset dialogue index
        dialogueIndex = 0;


        // Disable the next dialogue button UI
        if (nextDialogueUI != null) nextDialogueUI.SetActive(false);

        // Interaction is finished
        isInteracting = false;

        // Optionally, re-enable the Reticle and holdPos here if required
        ResetUI();
    }

    private void OnDestroy()
    {
        if (interactionInput != null)
        {
            interactionInput.action.performed -= OnInteractionPerformed; // Unbind interaction action
        }
    }

    public void ResetUI()
    {
        // Enable reticle and holdPos
        if (Reticle != null) Reticle.SetActive(true);
        if (holdPos != null) holdPos.SetActive(true);

        // Enable hotbar UI
        if (hotbarUI != null) hotbarUI.SetActive(true);

        // Disable InteractUI, shopUI, and nextDialogueUI
        if (InteractUI != null) InteractUI.SetActive(false);
        if (nextDialogueUI != null) nextDialogueUI.SetActive(false);
        if (ExitUi != null) ExitUi.SetActive(false);

        // Ensure all dialogue UI elements are disabled
        foreach (var ui in dialogueUI)
        {
            ui.SetActive(false);
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
