using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject crosshairs;
	public GameObject pausePanel;       // The pause panel UI
	//public GameObject settingsPanel;   // The settings panel (optional)
	//private bool isPaused = false;     // Tracks if the game is paused


	public void ResumeGame()
	{
		if (pausePanel != null)
		{
			crosshairs.SetActive(true);
			pausePanel.SetActive(false);   // Hide the pause panel
			Time.timeScale = 1f;           // Resume the game
			Cursor.visible = false;        // Hide the cursor
			Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
			//isPaused = false;              // Set pause state
		}
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;               // Ensure the game is unpaused
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1f;               // Ensure the game is unpaused
		SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your scene name
	}

	//public void OpenSettings()
	//{
	//	if (settingsPanel != null)
	//	{
	//		pausePanel.SetActive(false);   // Hide the pause panel
	//		settingsPanel.SetActive(true); // Show the settings panel
	//	}
	//}

	//public void CloseSettings()
	//{
	//	if (settingsPanel != null)
	//	{
	//		settingsPanel.SetActive(false); // Hide the settings panel
	//		pausePanel.SetActive(true);     // Show the pause panel
	//	}
	//}

	public void QuitGame()
	{
		Debug.Log("Quit Game");            // For testing in the Unity Editor
		Application.Quit();                // Quits the game (works in builds)
	}
}
