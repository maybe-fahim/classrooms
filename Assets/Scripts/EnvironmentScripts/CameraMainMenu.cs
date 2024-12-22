using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    public float movementDistance = 4f; // The distance to move left and right
    public float speed = 1f; // Speed of the movement

    private float timeCounter = 0f;
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Increment the time counter based on speed
        timeCounter += Time.deltaTime * speed;

        // Calculate the new position using a sine wave
        float offsetX = Mathf.Sin(timeCounter) * movementDistance;

        // Update the object's position
        transform.position = startPosition + new Vector3(offsetX, 0, 0);
    }
}
