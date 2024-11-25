using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform sphere; // The rotating sphere
    public Transform player; // The player object
    public float rotationSmoothness = 5f; // Smoothness of the sphere's rotation
    public Vector3 lookAtOffset = new Vector3(0f, 1.5f, 0f); // Offset to target the player's head

    void Start()
    {
        if (sphere == null || player == null)
        {
            Debug.LogError("Missing references! Please assign the sphere and player.");
            return;
        }
    }

    void LateUpdate()
    {
        if (sphere != null && player != null)
        {
            // Calculate the direction to the player's position with an offset
            Vector3 targetPosition = player.position + lookAtOffset;
            Vector3 directionToPlayer = targetPosition - sphere.position;

            // Smoothly rotate the sphere to look at the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            sphere.rotation = Quaternion.Slerp(sphere.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }
    }
}
