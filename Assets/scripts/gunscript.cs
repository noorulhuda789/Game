using System.Collections;
using UnityEngine;
using TMPro;

using System;



public class GunScript : MonoBehaviour
{
	// Gun stats
	public int damage;
	public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
	public int magazineSize, bulletsPerTap;
	public bool allowButtonHold;
	private int bulletsLeft, bulletsShot;

	// Bools
	private bool shooting, readyToShoot, reloading;

	// Reference
	public Camera playerCamera;
	public Transform attackPoint;
	public RaycastHit rayHit;
	public LayerMask whatIsEnemy;

	// Graphics
	public GameObject muzzleFlash, bulletHoleGraphic;
	public CamShake camShake;
	public float camShakeMagnitude, camShakeDuration;
	

	// Rigidbody Reference
	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>(); // Get Rigidbody
		bulletsLeft = magazineSize;
		readyToShoot = true;
	}

	private void Update()
	{
		HandleInput();

		
	}

	private void HandleInput()
	{
		// Handle shooting input
		

		// Handle reloading input
		if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
		{
			Reload();
		}

		// Shooting logic
		if (readyToShoot && Input.GetKeyDown(KeyCode.F) && !reloading && bulletsLeft > 0)
		{
			bulletsShot = bulletsPerTap;
			Shoot();
		}
	}

	public void Shoot()
	{
		readyToShoot = false;

		// Spread
		float x = UnityEngine.Random.Range(-spread, spread);
		float y = UnityEngine.Random.Range(-spread, spread);

		// Calculate direction with spread
		Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);
		Debug.DrawRay(playerCamera.transform.position, direction * range, Color.red, 2f);
		RaycastHit rayHit;
		// Raycast
		UnityEngine.Debug.Log("CanSee");
		if (Physics.Raycast(playerCamera.transform.position, direction, out rayHit, range, whatIsEnemy))
		{
			Debug.Log(rayHit.collider.name);

			if (rayHit.collider.CompareTag("Enemy"))
			{
				rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
			}
		}

		// Camera shake
		camShake.Shake(camShakeDuration, camShakeMagnitude);

		// Graphics
		if (rayHit.collider != null)
		{
			Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.identity);
		}
		Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

		// Physics-based recoil
		ApplyRecoil();

		bulletsLeft--;
		bulletsShot--;

		// Reset shooting delay
		Invoke(nameof(ResetShot), timeBetweenShooting);

		// Shoot again if bullets remain in tap
		if (bulletsShot > 0 && bulletsLeft > 0)
		{
			Invoke(nameof(Shoot), timeBetweenShots);
		}
	}

	private void ApplyRecoil()
	{
		// Add a recoil force to the gun Rigidbody
		if (rb != null)
		{
			Vector3 recoilDirection = -playerCamera.transform.forward; // Backward force
			rb.AddForce(recoilDirection * 10f, ForceMode.Impulse); // Adjust force value as needed
		}
	}

	private void ResetShot()
	{
		readyToShoot = true;
	}

	private void Reload()
	{
		reloading = true;
		Invoke(nameof(FinishReload), reloadTime);
	}

	private void FinishReload()
	{
		bulletsLeft = magazineSize;
		reloading = false;
	}
}
