using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlightSource;
    [SerializeField]
    private InputActionReference useInput;
    public float batteryLife = 100;
    void Start()
    {
        useInput.action.performed += Use;
    }

    void Update()
    {
        if (flashlightSource.activeSelf)
        {
            batteryLife-= 1 * Time.deltaTime;
        }
        if (batteryLife <= 0)
        {
            flashlightSource.SetActive(false);
        }



        
    }

    void Use(InputAction.CallbackContext obj)
    {
        flashlightSource.SetActive(!flashlightSource.activeSelf);
    }
}
