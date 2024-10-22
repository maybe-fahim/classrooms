using UnityEngine;

public class LeverDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;  // The door associated with the lever
    private bool leverFlicked = false;  // Track if the lever has been flicked
    private Collider doorCollider;

    [SerializeField] private Animator leverAnimator = null;
    [SerializeField] private GameObject leverHinge;

    // Check if the lever has already been flicked
    public bool IsLeverFlicked()
    {
        return leverFlicked;
    }

    // Flick the lever and activate the door
    public void FlickLever()
    {
        // Change lever state
        leverFlicked = true;
        Debug.Log("Lever has been flicked!");

        leverAnimator.Play("OpenLever", 0, 0.0f);

        // Activate the door's collider
        if (door != null)
        {
            doorCollider = door.GetComponent<Collider>();
            if (doorCollider != null)
            {
                // Enable the door's collider
                doorCollider.enabled = true;
                Debug.Log("Door collider enabled.");
            }
            else
            {
                Debug.LogWarning("No Collider found on the door.");
            }
        }
        else
        {
            Debug.LogWarning("No door assigned to this lever.");
        }
    }
}
