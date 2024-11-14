using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerDoorController : MonoBehaviour
{
    // references for animators and mesh coliders
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Animator myHandle = null;

   
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Play the door opening animation
            myDoor.Play("openDoor", 0, 0.0f);

            // Play the handle turning animation
            myHandle.Play("openDoorHandle", 0, 0.0f);

            // Disable the open door trigger
            gameObject.SetActive(false);

            // disable its MeshCollider
            if (targetObject != null)
            {
                targetObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
}
