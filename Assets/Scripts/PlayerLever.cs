using UnityEngine;

public class PlayerLever : MonoBehaviour
{
    public float interactRange = 5f;  // Interaction range for the lever
    public Camera playerCamera;       // Camera for the player's first-person view

    void Update()
    {
        // Check if the player presses the "E" key
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            // Raycast from the camera, in the direction the player is looking
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactRange))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);  // Log what object was hit

                // Check if the hit object has the LeverDoor component
                LeverDoor lever = hit.transform.GetComponent<LeverDoor>();

                if (lever != null)
                {
                    Debug.Log("Lever found! Checking state...");

                    // Check if the lever has already been flicked
                    if (!lever.IsLeverFlicked())
                    {
                        // Flick the lever (change its state)
                        Debug.Log("Lever is not flicked yet. Flicking it now...");
                        lever.FlickLever();
                    }
                    else
                    {
                        Debug.Log("This lever has already been flicked.");
                    }
                }
                else
                {
                    Debug.Log("No lever found on the object hit.");
                }
            }
            else
            {
                Debug.Log("Raycast didn't hit anything.");
            }
        }
    }
}
