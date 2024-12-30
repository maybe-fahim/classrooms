using UnityEngine;
using UnityEngine.Video;

public class LightningFlicker : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign your VideoPlayer in the Inspector
    public Light lightningLight;   // Assign your Point Light in the Inspector

    // Define the lightning times (in seconds)
    private float[] lightningTimes = new float[]
    {
        0.3f,    // 00:00:00:18
        2.5f,    // 00:00:02:30
        3.383f,  // 00:00:03:23
        5.183f,  // 00:00:05:11
        6.283f,  // 00:00:06:17
        8.483f,  // 00:00:08:29
        9.317f,  // 00:00:09:19
        11.2f,   // 00:00:11:12
        12.283f, // 00:00:12:17
        14.5f,   // 00:00:14:30
        15.4f,   // 00:00:15:24
        17.2f    // 00:00:17:12
    };

    private int currentIndex = 0; // Track which lightning time to trigger

    void Update()
    {
        // Check if the video is playing
        if (videoPlayer.isPlaying)
        {
            float currentTime = (float)videoPlayer.time; // Get current video time

            // Check if we've reached the next lightning time
            if (currentIndex < lightningTimes.Length && currentTime >= lightningTimes[currentIndex])
            {
                StartCoroutine(FlickerLightning()); // Trigger the flicker
                currentIndex++; // Move to the next lightning time
            }

            // Reset when video loops
            if (currentTime < lightningTimes[0] && currentIndex > 0)
            {
                currentIndex = 0; // Restart sequence
            }
        }
    }

    private System.Collections.IEnumerator FlickerLightning()
    {
        // Flicker pattern: rapid on/off flashes
        lightningLight.enabled = true;
        yield return new WaitForSeconds(0.03f); // Flash on for 30ms
        lightningLight.enabled = false;
        yield return new WaitForSeconds(0.05f); // Pause for 50ms
        lightningLight.enabled = true;
        yield return new WaitForSeconds(0.04f); // Flash on for 40ms
        lightningLight.enabled = false;
        yield return new WaitForSeconds(0.03f); // Pause for 30ms
        lightningLight.enabled = true;
        yield return new WaitForSeconds(0.05f); // Final flash on for 50ms
        lightningLight.enabled = false;
    }
}
