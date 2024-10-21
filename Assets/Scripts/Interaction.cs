using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickablelayerMask;
    [SerializeField]
    private Transform playerCameraTransform;
    [SerializeField]
    private GameObject pickUpUI;
    [SerializeField]
    [Min(1)]
    private float hitRange = 3f;

    [SerializeField]
    private InputActionReference interactionInput, dropInput;
    
    private RaycastHit hit;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private GameObject inHandItem;
    private Vector3 originalScale;

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if(hit.collider.GetComponent<Item>())
            {
            Debug.Log("Picked up " + hit.collider.name);
            inHandItem = hit.collider.gameObject;
            inHandItem.transform.position = Vector3.zero;
            inHandItem.transform.rotation = Quaternion.identity;
            originalScale = inHandItem.transform.localScale;
            inHandItem.transform.SetParent(pickUpParent.transform,false);
            
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            return;
            }
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            inHandItem.transform.localScale = originalScale;
            inHandItem = null;
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
    private void Update()
    {
        

        if(hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }

        if(inHandItem != null)
        {
            return;
        }

        if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickablelayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);
        
        }
    }

}
