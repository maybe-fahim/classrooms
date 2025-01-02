using UnityEngine;

public class RandomCameraToggler : MonoBehaviour
{
    [Tooltip("The probability (0 to 1) that each camera is turned on.")]
    [Range(0f, 1f)]
    public float cameraOnProbability = 0.5f; // Default 50% chance

    void Start()
    {
        int childCount = transform.childCount;

        // Loop through each child and decide if it's on or off
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            
            // Decide randomly if this camera is on or off based on probability
            bool shouldTurnOn = Random.value < cameraOnProbability;
            childTransform.gameObject.SetActive(shouldTurnOn);
        }
    }
}
