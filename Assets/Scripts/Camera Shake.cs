using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Variables for camera shake intensity and frequency
    public float shakeIntensity = 20f;    // How much the camera shakes
    public float shakeFrequency = 5f;  // How often the camera shakes

    private Vector3 originalPosition;
    private bool isShaking = false;
    private float timer = 0f;

    void Start()
    {
        // Save the initial position of the camera
        originalPosition = transform.localPosition;
    }

    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    public void StopShake()
    {
        // Stop the shaking coroutine
        StopAllCoroutines();
        transform.localPosition = originalPosition;
        isShaking = false;
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        while (true)
        {
            timer += Time.deltaTime * 5f;
            // Apply a small random offset to the camera's position
            transform.localPosition = new Vector3(
                originalPosition.x,
                originalPosition.y + Mathf.Sin(timer * shakeFrequency) * shakeIntensity,
                originalPosition.z
            );
            // Wait for a short time before applying the next shake
            yield return null;
        }
        
    }
}

