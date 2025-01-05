using UnityEngine;

public class CeilingLightFlicker : MonoBehaviour
{
    public Light light1; 
    public Light light2; 
    public Renderer lightModelRenderer; 
    public string emissiveMaterialName = "(Mat)EmissiveWarm"; 
    public float normalIntensity = 2f; 
    public float dimIntensity = 0.5f; 
    public float flickerDuration = 0.1f; 
    public float timeBetweenFlickers = 2f; 
    public bool randomizeTiming = true; 

    [Range(0f, 1f)] public float probabilityOn = 0.5f;
    [Range(0f, 1f)] public float probabilityFlicker = 0.3f;
    [Range(0f, 1f)] public float probabilityOff = 0.2f;

    private Material emissiveMaterial;
    private enum LightState { On, Flicker, Off }
    private LightState currentState;

    private float nextFlickerTime; 
    private bool isFlickering = false;
    private float flickerEndTime; 

    // =============== ADDED / UPDATED ===============
    // We do it in Awake() to grab manager's values before Start() runs.
    void Awake()
    {
        // Attempt to find a DifficultyManager in the scene
        DifficultyManager dm = FindObjectOfType<DifficultyManager>();
        if (dm != null)
        {
            probabilityOn = dm.GetChanceLightOn();
            probabilityFlicker = dm.GetChanceLightFlicker();
            probabilityOff = dm.GetChanceLightOff();
        }
        else
        {
            Debug.LogWarning("CeilingLightFlicker: No DifficultyManager found in scene. Using local inspector values.");
        }
    }
    // ===============================================

    void Start()
    {
        if (light1 == null || light2 == null || lightModelRenderer == null)
        {
            Debug.LogError("Both Light1, Light2, and LightModel references must be assigned!");
            return;
        }

        // Fetch the emissive material from the renderer
        emissiveMaterial = GetEmissiveMaterial();

        // **Now that probabilities are set**, normalize them
        NormalizeProbabilities(); 
        currentState = GetRandomState(); // Randomly decide the initial state

        // Randomize or schedule flicker timing
        if (randomizeTiming)
        {
            nextFlickerTime = Time.time + Random.Range(0f, timeBetweenFlickers);
        }
        else
        {
            ScheduleNextFlicker();
        }

        // Initialize behavior based on state
        if (currentState == LightState.On)
        {
            SetLightsIntensity(normalIntensity);
            SetEmissiveState(true);
        }
        else if (currentState == LightState.Off)
        {
            SetLightsIntensity(0f); 
            SetEmissiveState(false);
        }
        // If Flicker, we'll handle that in Update when the time triggers.
    }

    void Update()
    {
        if (isFlickering)
        {
            HandleFlicker();
        }
        else
        {
            if (Time.time >= nextFlickerTime)
            {
                currentState = GetRandomState(); // Re-evaluate random state

                if (currentState == LightState.Flicker)
                {
                    StartFlickering();
                }
                else if (currentState == LightState.On)
                {
                    SetLightsIntensity(normalIntensity);
                    SetEmissiveState(true);
                }
                else if (currentState == LightState.Off)
                {
                    SetLightsIntensity(0f);
                    SetEmissiveState(false);
                }
                ScheduleNextFlicker();
            }
        }
    }

    private void HandleFlicker()
    {
        if (Time.time >= flickerEndTime)
        {
            // Flicker done, return to normal
            SetLightsIntensity(normalIntensity);
            SetEmissiveState(true);
            isFlickering = false;
        }
        else
        {
            bool isBright = Random.value > 0.5f;
            SetLightsIntensity(isBright ? dimIntensity : normalIntensity);
            SetEmissiveState(isBright);
        }
    }

    private void StartFlickering()
    {
        isFlickering = true;
        flickerEndTime = Time.time + flickerDuration;
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
        float randomValue = Random.value; 
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
