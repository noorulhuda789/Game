using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool UsesHealthBar = true;
    public bool UsesHealthCounter = true;
    public Image healthBarFill;
    public Text healthCounterText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    // Method to update the player's health
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;

        // Ensure health doesn't exceed maximum
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update the UI elements
    private void UpdateUI()
    {
        if (UsesHealthBar && healthBarFill != null)
            UpdateHealthBar();

        if (UsesHealthCounter && healthCounterText != null)
            UpdateHealthCounter();
    }

    // Method to update the health bar UI
    private void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = fillAmount;
    }

    // Method to update the health counter text
    private void UpdateHealthCounter()
    {
        healthCounterText.text = currentHealth.ToString();
    }

    // Method to handle player death
    private void Die()
    {
        // Add any death-related logic here
        Debug.Log("Player has died.");
    }
}
