using MagicPigGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public GameObject HealthUI;
	public GameObject ProgressUI;
	public GameObject crosshairs;
	public GameObject fillMask;
	public ProgressBar healthBar; // Reference to the Progress Bar script
	public GameObject gameOverPanel;
	public GameObject pausePanel;

	private RawImage fillImage; // Reference to the health bar's Fill Image
	private float maxHealth = 100f;
	private float currentHealth;
	private Color fullHealthColor = new Color(0f, 0.9f, 0f);       // Bright Green (#00FF00)
	private Color midHealthColor = new Color(0.6f, 0.8f, 0.2f);    // Yellow-Green (#9ACD32)
	private Color lowHealthColor = new Color(1f, 0.27f, 0f);       // Red-Orange (#FF4500)
	private bool isGameOver = false; // Flag to check if the game is over
	private bool isPaused = false; // Flag to track pause state

	void Start()
	{
		// Get the Image component from the Fill Mask's child object
		fillImage = fillMask.transform.GetChild(0).GetComponent<RawImage>();
		currentHealth = maxHealth;
		UpdateHealthBar();

		// Ensure the Game Over panel is hidden at the start
		if (gameOverPanel != null)
		{
			HealthUI.SetActive(true);
			ProgressUI.SetActive(true);
			crosshairs.SetActive(true);
			gameOverPanel.SetActive(false);
			pausePanel.SetActive(false);
		}
	}

	void Update()
	{
		// Check for the pause key (K)
		if (Input.GetKeyDown(KeyCode.K) && !isGameOver)
		{
			TogglePause();
		}
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays between 0 and max
		UpdateHealthBar();

		if (currentHealth <= 0)
		{
			GameOver(); // Trigger Game Over when health reaches zero
		}
	}

	private void UpdateHealthBar()
	{
		float healthPercentage = currentHealth / maxHealth; // Normalize health to 0-1
		healthBar.SetProgress(healthPercentage); // Update the bar visually

		if (healthPercentage > 0.5f)
		{
			// Transition from fullHealthColor to midHealthColor
			float t = (healthPercentage - 0.5f) / 0.5f;
			fillImage.color = Color.Lerp(midHealthColor, fullHealthColor, t);
		}
		else
		{
			// Transition from midHealthColor to lowHealthColor
			float t = healthPercentage / 0.5f;
			fillImage.color = Color.Lerp(lowHealthColor, midHealthColor, t);
		}
	}

	private void GameOver()
	{
		HealthUI.SetActive(false);
		ProgressUI.SetActive(false);
		crosshairs.SetActive(false);

		// Activate the Game Over UI panel
		if (gameOverPanel != null)
		{
			gameOverPanel.SetActive(true);
			isGameOver = true;

			// Show and unlock the cursor
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			// Pause the game
			Time.timeScale = 0f;
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			TakeDamage(10f); // Reduce health by 10
		}
	}

	private void TogglePause()
	{
		isPaused = !isPaused;

		if (isPaused)
		{
			// Show pause menu and unlock the cursor
			pausePanel.SetActive(true);
			crosshairs.SetActive(false);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0f; // Pause the game

		}
		else
		{
			resumeGame();
		}
	}

	private void resumeGame()
	{
		// Hide pause menu and lock the cursor
		pausePanel.SetActive(false);
		crosshairs.SetActive(true);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f; // Resume the game
	}
}
