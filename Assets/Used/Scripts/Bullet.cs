using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// You can use this to adjust bullet behavior, e.g., damage, effects, etc.
	//public float bulletLifetime = 3f; // Time after which the bullet gets destroyed if not colliding
	[SerializeField]private GameObject player;
	private void Start()
	{
		//// Optionally destroy bullet after a set time to avoid it staying forever
		//Destroy(gameObject, bulletLifetime);
	}


	private void OnCollisionEnter(Collision collision)
	{
		// Destroy the bullet when it collides with any object
		Destroy(gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Debug.Log("Player hurt");
			player.GetComponent<PlayerHealth>().TakeDamage(10f); // Apply damage to the player
		}
	}
}
