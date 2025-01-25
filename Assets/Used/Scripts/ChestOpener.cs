using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChestOpener : MonoBehaviour
{
	private Animator chestAnimator; // Reference to the Animator on the Cube
	private bool playerInRange = false; // Check if the player is near the Cube

	[SerializeField] private GameObject Cube; // Drag and drop the Cube GameObject in the Inspector

	void Start()
	{
		// Get the Animator component directly from the Cube object
		if (Cube != null)
		{
			chestAnimator = Cube.GetComponent<Animator>();

			if (chestAnimator == null)
			{
				
			}
		}
		else
		{
			
		}
	}

	void Update()
	{
		// Check if player is in range and the E key is pressed
		if (playerInRange && Input.GetKeyDown(KeyCode.E))
		{
			if (chestAnimator != null)
			{
				// Trigger the chest opening animation
				chestAnimator.SetTrigger("chestOpening");
			
			}
			else
			{
			
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if the colliding object is the player
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		// Check if the player has exited the range
		if (other.CompareTag("Player"))
		{
			playerInRange = false;
		
		}
	}
}
