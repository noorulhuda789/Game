using System.Collections;

using UnityEngine;

public class ObjectRangeTrigger : MonoBehaviour
{
	public GameObject player; // Reference to the player object
	public float scaleMultiplier = 1.5f;  // The multiplier by which the object scales up
	public float scaleDuration = 1f;  // The duration to scale back to original size after leaving range
	public float triggerRange = 5f;  // The range within which the scaling occurs

	private Vector3 originalScale; // To store the original scale of the object
	private bool isPlayerInRange = false;

	private void Start()
	{
		// Store the original scale of the object
		originalScale = transform.localScale;
	}

	private void Update()
	{
		// Check the distance between the player and the object
		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (distance <= triggerRange && !isPlayerInRange)
		{
			// Player is within range, scale the object up
			isPlayerInRange = true;
			transform.localScale = originalScale * scaleMultiplier;

			Debug.Log("Object scaled up.");
		}
		else if (distance > triggerRange && isPlayerInRange)
		{
			// Player is out of range, scale the object back to original size
			isPlayerInRange = false;
			StartCoroutine(ScaleBackToOriginal());
		}
	}

	private IEnumerator ScaleBackToOriginal()
	{
		// Gradually scale back to the original scale over time
		float elapsedTime = 0f;

		while (elapsedTime < scaleDuration)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, originalScale, elapsedTime / scaleDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure it ends exactly at the original scale
		transform.localScale = originalScale;

		Debug.Log("Object scaled back to original size.");
	}
}
