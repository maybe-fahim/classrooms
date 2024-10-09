using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;

    [Header("Configurations")]
    public float walkSpeed;

    [Header("Crouching")]
    public float crouchSpeed;
    private Vector3 crouchScale = new Vector3(0.5f, 0.5f, 0.5f);
    private float startYScale;
    private float startWalkSpeed;
    


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startYScale = transform.localScale.y;
        startWalkSpeed = walkSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X")* 2f);

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = crouchScale;
            walkSpeed = crouchSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1, startYScale, 1);
            walkSpeed = startWalkSpeed;
        }

    }

    void FixedUpdate()
    {
        Vector3 newVelocity = Vector3.up * rb.velocity.y;
        newVelocity.x = Input.GetAxis("Horizontal") * walkSpeed;
        newVelocity.z = Input.GetAxis("Vertical") * walkSpeed;
        rb.velocity = transform.TransformDirection(newVelocity);
    }

    void LateUpdate()
    {
        // Vertical Rotation
        Vector3 e = head.eulerAngles;
        e.x-= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x, -85f, 85f);
        head.eulerAngles = e;
    }

    public static float RestrictAngle(float angle, float angleMin, float angleMax)
    {
        if ( angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        if (angle > angleMax)
        {
            angle = angleMax;
        }
        else if (angle < angleMin)
        {
            angle = angleMin;
        }
        return angle;
    }

    
}
