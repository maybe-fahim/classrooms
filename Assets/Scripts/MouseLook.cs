using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    private PlayerControls controls;
    public float mouseSenstivity = 40f;
    private Vector2 mouseLook;
    private float xRotation = 0f;
    private Transform playerBody;

    void Awake()
    {
        playerBody = transform.parent;
        controls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update()
    {
        Look();
    }

    private void Look()
    {
        mouseLook = controls.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseLook.x * mouseSenstivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSenstivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}

 