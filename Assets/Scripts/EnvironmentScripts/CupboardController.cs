using UnityEngine;
using UnityEngine.InputSystem;

public class CupboardController : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] private Animator cupboardAnimator;
    [SerializeField] private string openTriggerName = "Open";
    
    private bool isOpen = false;

    /// <summary>
    /// Called by the new Input System (Interaction action).
    /// </summary>
    /// <param name="value">Contains data about the input event.</param>
    public void OnInteract(InputValue value)
    {
        // Only act if the button was pressed and the cupboard is not already open.
        if (!value.isPressed || isOpen) return;

        // Trigger the Open animation.
        cupboardAnimator.SetTrigger(openTriggerName);
        isOpen = true;
    }
}
