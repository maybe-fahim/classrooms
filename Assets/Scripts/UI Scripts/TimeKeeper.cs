using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
    public Image timerBar;
    public float time = 120f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 120)
        {
            time = 120;
        } 
        if (time > 0)
        {
            // Decrease the remaining time
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0, 120);
            timerBar.fillAmount = time / 120f;
            // Check if time has run out
            if (time <= 0)
            {
                Debug.Log("You died");
            }
        }
    }
}
