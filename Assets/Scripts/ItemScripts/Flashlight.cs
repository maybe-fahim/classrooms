using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlightSource;
    [SerializeField]
    private InputActionReference useInput;
    public float batteryLife = 100;
    public Image batteryUI;
    public GameObject batteryUIObject;
    void Start()
    {
        useInput.action.performed += Use;
    }

    void Update()
    {
        if (flashlightSource.activeSelf)
        {
            batteryLife-= 1 * Time.deltaTime;
            batteryUIObject.SetActive(true);
            batteryUI.fillAmount = batteryLife / 100;
        }
        else
        {
            batteryUIObject.SetActive(false);
        }

        if (batteryLife <= 0)
        {
            flashlightSource.SetActive(false);
            batteryUIObject.SetActive(true);
        }

        if (batteryLife >= 100)
        {
            batteryLife = 100;
        }



        
    }

    void Use(InputAction.CallbackContext obj)
    {
        flashlightSource.SetActive(!flashlightSource.activeSelf);
    }
}
