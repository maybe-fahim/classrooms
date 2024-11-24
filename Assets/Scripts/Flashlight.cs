using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlightSource;
    [SerializeField]
    private InputActionReference useInput;
    void Start()
    {
        useInput.action.performed += Use;
    }

    void Update()
    {

        
    }

    void Use(InputAction.CallbackContext obj)
    {
        flashlightSource.SetActive(!flashlightSource.activeSelf);
    }
}
