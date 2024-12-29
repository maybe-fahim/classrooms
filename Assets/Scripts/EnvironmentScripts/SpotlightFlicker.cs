using UnityEngine;

public class SpotlightFlicker : MonoBehaviour
{
    public Light spotlight;           // Reference to the spotlight
    public float normalIntensity = 2f; // Normal light intensity
    public float dimIntensity = 0.5f; // Dim light intensity during flicker
    public float flickerDuration = 0.1f; // How long a single flicker lasts
    public float timeBetweenFlickers = 2f; // Average time between flicker events
    public bool randomizeTiming = true; // Randomize timing between flickers

    private float nextFlickerTime;
    private bool isFlickering = false;
    private float flickerEndTime;

    void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
        }

        ScheduleNextFlicker();
    }

    void Update()
    {
        if (isFlickering)
        {
            // If currently flickering, handle the flicker duration
            if (Time.time >= flickerEndTime)
            {
                spotlight.intensity = normalIntensity; // Return to normal
                isFlickering = false;
                ScheduleNextFlicker();
            }
            else
            {
                // Flicker between dim and bright states
                spotlight.intensity = Random.value > 0.5f ? dimIntensity : normalIntensity;
            }
        }
        else
        {
            // Check if it's time to start the next flicker
            if (Time.time >= nextFlickerTime)
            {
                StartFlickering();
            }
        }
    }

    private void StartFlickering()
    {
        isFlickering = true;
        flickerEndTime = Time.time + flickerDuration; // Set how long the flicker lasts
    }

    private void ScheduleNextFlicker()
    {
        nextFlickerTime = Time.time + (randomizeTiming ? Random.Range(1f, timeBetweenFlickers) : timeBetweenFlickers);
    }
}