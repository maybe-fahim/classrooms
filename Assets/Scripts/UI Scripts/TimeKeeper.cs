using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
    [SerializeField] private Image timerBar;

    private float currentTime;
    private float maxTime;

    void Awake()
    {
        // If you also use DifficultyManager for startTime, maxTime:
        DifficultyManager dm = FindObjectOfType<DifficultyManager>();
        if (dm != null)
        {
            maxTime = dm.GetMaxTime();
            currentTime = maxTime;
        }
        else
        {
            maxTime = 120f;
            currentTime = maxTime;
        }
    }

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0f, maxTime);

            if (timerBar != null)
            {
                timerBar.fillAmount = currentTime / maxTime;
            }
        }
    }

    // PUBLIC METHOD to add time
    public void AddTime(float extra)
    {
        currentTime += extra;
        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);

        Debug.Log($"TimeKeeper: Added {extra} seconds. Current time is now: {currentTime}");
    }
}
