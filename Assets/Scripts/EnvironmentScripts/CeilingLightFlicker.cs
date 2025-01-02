using UnityEngine;

public class CeilingLightFlicker : MonoBehaviour
{
    public Light light1; // Reference to Light1
    public Light light2; // Reference to Light2
    public Renderer lightModelRenderer; // Reference to the LightModel Renderer
    public string emissiveMaterialName = "(Mat)EmissiveWarm"; // Name of the emissive material
    public float normalIntensity = 2f; // Normal light intensity
    public float dimIntensity = 0.5f; // Dim light intensity during flicker
    public float flickerDuration = 0.1f; // How long a single flicker lasts
    public float timeBetweenFlickers = 2f; // Average time between flicker events
    public bool randomizeTiming = true; // Randomize timing between flickers

    [Range(0f, 1f)] public float probabilityOn = 0.5f; // Probability for "On" state
    [Range(0f, 1f)] public float probabilityFlicker = 0.3f; // Probability for "Flicker" state
    [Range(0f, 1f)] public float probabilityOff = 0.2f; // Probability for "Off" state

    private Material emissiveMaterial;
    private enum LightState { On, Flicker, Off }
    private LightState currentState;
    private float nextFlickerTime;
    private bool isFlickering = false;
    private float flickerEndTime;

    void Start()
    {
        if (light1 == null || light2 == null || lightModelRenderer == null)
        {
            Debug.LogError("Both Light1, Light2, and LightModel references must be assigned!");
            return;
        }

        // Fetch the emissive material from the renderer
        emissiveMaterial = GetEmissiveMaterial();

        NormalizeProbabilities(); // Ensure probabilities sum to 1
        currentState = GetRandomState(); // Randomly decide the initial state

        // Initialize behavior based on state
        if (currentState == LightState.On)
        {
            SetLightsIntensity(normalIntensity);
            SetEmissiveState(true);
        }
        else if (currentState == LightState.Off)
        {
            SetLightsIntensity(0f); // Turn off
            SetEmissiveState(false);
        }
        else if (currentState == LightState.Flicker)
        {
            ScheduleNextFlicker();
        }
    }

    void Update()
    {
        if (currentState == LightState.Flicker)
        {
            HandleFlicker();
        }
    }

    private void HandleFlicker()
    {
        if (isFlickering)
        {
            // If currently flickering, handle the flicker duration
            if (Time.time >= flickerEndTime)
            {
                SetLightsIntensity(normalIntensity); // Return to normal
                SetEmissiveState(true);
                isFlickering = false;
                ScheduleNextFlicker();
            }
            else
            {
                // Flicker between dim and bright states
                bool isBright = Random.value > 0.5f;
                SetLightsIntensity(isBright ? dimIntensity : normalIntensity);
                SetEmissiveState(isBright);
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

    private void SetLightsIntensity(float intensity)
    {
        light1.intensity = intensity;
        light2.intensity = intensity;
    }

    private void SetEmissiveState(bool isOn)
    {
        if (emissiveMaterial != null)
        {
            if (isOn)
            {
                emissiveMaterial.EnableKeyword("_EMISSION");
                emissiveMaterial.SetColor("_EmissionColor", Color.white * normalIntensity);
            }
            else
            {
                emissiveMaterial.DisableKeyword("_EMISSION");
                emissiveMaterial.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    private Material GetEmissiveMaterial()
    {
        foreach (var mat in lightModelRenderer.sharedMaterials)
        {
            if (mat.name.Contains(emissiveMaterialName))
            {
                return mat;
            }
        }
        Debug.LogError($"Material '{emissiveMaterialName}' not found on LightModel Renderer!");
        return null;
    }

    private LightState GetRandomState()
    {
        float randomValue = Random.value; // Generate a random value between 0 and 1
        if (randomValue < probabilityOn)
        {
            return LightState.On;
        }
        else if (randomValue < probabilityOn + probabilityFlicker)
        {
            return LightState.Flicker;
        }
        else
        {
            return LightState.Off;
        }
    }

    private void NormalizeProbabilities()
    {
        float total = probabilityOn + probabilityFlicker + probabilityOff;
        if (Mathf.Approximately(total, 0f))
        {
            Debug.LogError("Probabilities must sum to a positive value. Resetting to defaults.");
            probabilityOn = 0.5f;
            probabilityFlicker = 0.3f;
            probabilityOff = 0.2f;
            total = 1f;
        }

        probabilityOn /= total;
        probabilityFlicker /= total;
        probabilityOff /= total;
    }
}
