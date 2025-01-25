using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
	public int health = 5;

	public void TakeDamage(int damage)
	{
		
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		Destroy(gameObject); // Destroy the enemy
	}
}
