using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
	private void Start()
	{
		Debug.Log("Cursor State: " + Cursor.lockState + ", Visible: " + Cursor.visible);

	}
	public void QuitGame()
	{
		Application.Quit(); 
	}
	public void MainMenuLoading()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}
}
