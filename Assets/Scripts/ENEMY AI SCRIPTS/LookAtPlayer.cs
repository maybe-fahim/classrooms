using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;       // Reference to the Player GameObject
    public Transform holder;       // Reference to the Holder GameObject
    public Transform camera;       // Reference to the Camera GameObject
    public Vector3 rotationOffset; // Offset for correcting the camera's rotation
    public Vector3 lookAtOffset;   // Offset to focus on the player's eyes
    public float rotationSpeed = 5f; // Speed of smoothing rotation

    void Update()
    {
        if (player != null && holder != null && camera != null)
        {
            // Calculate direction to the player, ignoring Y-axis for Holder rotation
            Vector3 direction = new Vector3(
                player.position.x - holder.position.x,
                0,
                player.position.z - holder.position.z
            );

            if (direction.sqrMagnitude > 0.001f)
            {
                // Smoothly rotate the Holder on XZ plane
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                holder.rotation = Quaternion.Slerp(
                    holder.rotation,
                    Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
                    Time.deltaTime * rotationSpeed
                );

                // Calculate the look-at target and smoothly rotate the camera
                Vector3 targetLookAt = player.position + lookAtOffset;

                // Look directly at the target
                Quaternion cameraTargetRotation = Quaternion.LookRotation(targetLookAt - camera.position);
                camera.rotation = Quaternion.Slerp(
                    camera.rotation,
                    cameraTargetRotation * Quaternion.Euler(rotationOffset),
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }
}
