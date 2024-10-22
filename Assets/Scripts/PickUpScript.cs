using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public GameObject holdPos;
    
    public float throwForce = 500f; // force at which the object is thrown
    public float pickUpRange = 5f;  // how far the player can pick up the object
    
    private GameObject heldObj;     // the object we pick up
    private Rigidbody heldObjRb;    // Rigidbody of the object we pick up
    private Vector3 originalScale;  // Store the original scale of the object
    private bool canDrop = true;    // Used to prevent throwing/dropping while rotating
    private int LayerNumber;        // Layer index for holding objects
    
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); // Change this if your hold layer has a different name

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Key to pick up/drop object
        {
            if (heldObj == null) // If not holding anything
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                        // Add to inventory hotbar
                        
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); // Prevent object clipping through walls
                    DropObject();
                }
            }
        }

        if (heldObj != null) // If holding an object
        {
            MoveObject(); // Keep object at hold position
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) // Throw object on left-click
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    public void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;  // Assign held object
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); // Assign Rigidbody
            originalScale = heldObj.transform.localScale; // Store the original scale

            // Set the object to be kinematic and parent it to the hold position
            heldObjRb.isKinematic = true;
            heldObj.transform.parent = holdPos.transform; // Parent to hold position
            heldObj.transform.localScale = originalScale;  // Ensure scale doesn't change
            heldObj.layer = LayerNumber; // Change to holding layer
            
            // Ignore collision with the player
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

            
        }
    }

    void DropObject()
    {
        // Re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

        // Set object back to default layer
        heldObj.layer = 0;

        // Set Rigidbody back to non-kinematic
        heldObjRb.isKinematic = false;

        // Unparent object from hold position
        heldObj.transform.parent = null;

        // Reset the scale to the original scale
        heldObj.transform.localScale = originalScale;

        // Drop the object just in front of the player
        Vector3 dropPosition = player.transform.position + player.transform.forward * 1.5f;
        heldObj.transform.position = dropPosition;

        // Clear reference to the held object
        heldObj = null;

        // Notify InventoryHotbarSystem to remove the item
        

        
    }

    void MoveObject()
    {
        // Keep object at hold position
        heldObj.transform.position = holdPos.transform.position;
    }

    void ThrowObject()
    {
        // Similar to DropObject, but adds force for throwing
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj.transform.localScale = originalScale; // Reset the scale
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
        // Notify InventoryHotbarSystem to remove the item
        
    }

    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); // Distance from holdPos to the camera
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); // Prevent clipping
        }
    }
}
