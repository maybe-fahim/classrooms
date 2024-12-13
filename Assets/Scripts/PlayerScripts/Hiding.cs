using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hiding : MonoBehaviour
{
    public GameObject mainPlayer;
    public GameObject hidingPlayer;
    public GameObject hideText;
    public GameObject unhideText;
    [SerializeField]
    private InputActionReference interactionInput;
    bool interactable, hiding;

    void Start()
    {
        interactionInput.action.performed += HideTrigger;
        interactable = false;
        hiding = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactable = true;
            hideText.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactable = false;
            hideText.SetActive(false);
        }
    }

    void HideTrigger(InputAction.CallbackContext obj)
    {
        if ((interactable == true) && (hiding == false))
        {
            mainPlayer.SetActive(false);
            hidingPlayer.SetActive(true);
            hiding = true;
            hideText.SetActive(false);
            unhideText.SetActive(true);
        }
        else if (hiding)
        {
            hidingPlayer.SetActive(false);
            mainPlayer.SetActive(true);
            hiding = false;
            hideText.SetActive(true);
            unhideText.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
