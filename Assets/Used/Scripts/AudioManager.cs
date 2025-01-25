using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip alertSound; // Assign the sound clip in the Inspector
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
	}

	public void PlayAlertSound()
	{
		if (alertSound != null)
		{
			audioSource.PlayOneShot(alertSound); // Play the alert sound
		}
	}
}
