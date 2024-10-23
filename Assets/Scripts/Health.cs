using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < maxHealth)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        
    }
    public void TakeDamage(int amount)
    {
        health -= amount;

        // Make sure the health doesn't go below zero
        if (health <= 0)
        {
            health = 0;
            // Trigger player death logic here (e.g., restart game, show game over screen, etc.)
            Debug.Log("Player has died!");
        }

        // Update the health display if you have one
        Debug.Log("Player Health: " + health);
    }
}
