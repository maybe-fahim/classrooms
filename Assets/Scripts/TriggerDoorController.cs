using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private GameObject targetObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myDoor.Play("DoorOpen", 0, 0.0f);
            gameObject.SetActive(false);

            if (targetObject != null)
            {
                targetObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}