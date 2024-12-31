using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [Header("Door/Handle References")]
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private Animator myHandle = null;
    [SerializeField] private GameObject targetObject = null;

    [Header("Audio")]
    [SerializeField] private AudioSource doorAudioSource;   // AudioSource component for the door
    [SerializeField] private AudioClip doorOpenClip;        // Door-opening sound clip

    private bool isTriggered = false; // Prevents multiple trigger activations

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the player and ensure it hasn’t already triggered
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Mark as triggered

            // Play the door animation
            myDoor.Play("openDoor", 0, 0.0f);

            // Play the handle animation
            myHandle.Play("openDoorHandle", 0, 0.0f);

            // Play the door-opening sound
            PlayDoorSound();

            // Disable the trigger's Box Collider instead of the whole GameObject
            BoxCollider triggerCollider = GetComponent<BoxCollider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            // Disable the target object's MeshCollider if assigned
            if (targetObject != null)
            {
                MeshCollider mc = targetObject.GetComponent<MeshCollider>();
                if (mc != null)
                {
                    mc.enabled = false;
                }
            }
        }
    }

    private void PlayDoorSound()
    {
        // Check if AudioSource and Clip are properly assigned
        if (doorAudioSource != null && doorOpenClip != null)
        {
            doorAudioSource.PlayOneShot(doorOpenClip);
        }
        else
        {
            Debug.LogWarning("DoorAudioSource or DoorOpenClip is not assigned!");
        }
    }
}
