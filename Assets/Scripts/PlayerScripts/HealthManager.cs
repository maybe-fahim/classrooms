using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public GameObject player;
    public GameObject deathScreen; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        if (healthAmount > 100)
        {
            healthAmount = 100;
        }
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
