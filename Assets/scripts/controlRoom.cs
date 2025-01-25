using System.Diagnostics;
using UnityEngine;

public class controlRoom : MonoBehaviour
{
	public Animator doorAnimator; // Reference to the Animator controlling the door

	private void OnCollisionEnter(Collision collision)
	{
		// Check if the colliding object is the player
		if (collision.collider.CompareTag("Player"))
		{
			
		}
	}
}
