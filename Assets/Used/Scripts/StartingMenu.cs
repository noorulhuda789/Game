using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
	public GameObject firstPanel;  // Reference to the first panel
	public GameObject secondPanel; // Reference to the second panel

	private void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Debug.Log("Cursor State: " + Cursor.lockState + ", Visible: " + Cursor.visible);

		// Hide the second panel at the start
		secondPanel.SetActive(false);
	}

	// Function to start the game
	public void GameStart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
	}

	// Function to quit the application
	public void DoQuit()
	{
		Application.Quit();
	}

	// Function to switch to the second panel
	public void ShowSecondPanel()
	{
		firstPanel.SetActive(false);  // Hide the first panel
		secondPanel.SetActive(true); // Show the second panel
	}

	// Function to go back to the first panel
	public void ShowFirstPanel()
	{
		secondPanel.SetActive(false); // Hide the second panel
		firstPanel.SetActive(true);  // Show the first panel
	}
}
