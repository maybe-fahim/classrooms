using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void TakeDamage(float amount)
    {
        healthAmount -= amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        Debug.Log("Player Health: " + healthAmount);
        healthBar.fillAmount = healthAmount / 100f;
    }
    public void heal(float amount)
    {
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
    }
}
