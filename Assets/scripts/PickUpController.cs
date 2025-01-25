
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	public Transform playerCamera;  // Reference to the player's camera.
	public Transform holdPosition; // Position where the object will be held.
	public float grabRange = 3f;   // Maximum distance to grab objects.
	public float throwForce = 10f; // Force applied when throwing objects.
	public Material highlightMaterial; // Material used for highlighting objects.
	public Material defaultMaterial;   // Default material of the object.

	private GameObject highlightedObject; // Currently highlighted object.
	private GameObject grabbedObject;    // Currently grabbed object.
	private Rigidbody grabbedRb;         // Rigidbody of the grabbed object.
	private Quaternion originalRotation; // Original rotation of the grabbed object.

	void Start()
	{
		Debug.Log("NewBehaviourScript is running.");
	}

	void Update()
	{
		HighlightObject();

		if (Input.GetKeyDown(KeyCode.P)) // Press P key to grab/release object
		{
			if (grabbedObject == null)
			{
				TryGrabObject();
			}
			else
			{
				ReleaseObject();
			}
		}

		if (Input.GetMouseButtonDown(1)) // Right Mouse Button to throw object
		{
			ThrowObject();
		}
	}

	void HighlightObject()
	{
		RaycastHit hit;
		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, grabRange))
		{
			GameObject target = hit.collider.gameObject;

			if (target.CompareTag("Grabbable"))
			{
				if (highlightedObject != target) // Highlight only if it's a new object
				{
					ResetHighlight(); // Reset previously highlighted object
					highlightedObject = target;

					// Change material to highlight
					Renderer renderer = highlightedObject.GetComponent<Renderer>();
					if (renderer != null)
					{
						renderer.material = highlightMaterial;
					}
				}
			}
			else
			{
				ResetHighlight();
			}
		}
		else
		{
			ResetHighlight();
		}
	}

	void ResetHighlight()
	{
		if (highlightedObject != null)
		{
			// Reset the material of the previously highlighted object
			Renderer renderer = highlightedObject.GetComponent<Renderer>();
			if (renderer != null)
			{
				renderer.material = defaultMaterial;
			}
			highlightedObject = null;
		}
	}

	void TryGrabObject()
	{
		RaycastHit hit;
		if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, grabRange))
		{
			if (hit.collider.gameObject.CompareTag("Grabbable") || hit.collider.gameObject.CompareTag("Key"))
			{
				grabbedObject = hit.collider.gameObject;
				grabbedRb = grabbedObject.GetComponent<Rigidbody>();

				if (grabbedRb != null)
				{
					originalRotation = grabbedObject.transform.rotation; // Store the original rotation
					Destroy(grabbedRb); // Remove Rigidbody component
					grabbedObject.AddComponent<GunScript>(); // Attach GunScript
					grabbedObject.transform.position = holdPosition.position;
					grabbedObject.transform.rotation = holdPosition.rotation;
					grabbedObject.transform.SetParent(holdPosition);
				}
			}

			if (hit.collider.gameObject.CompareTag("door"))
			{
				Animator doorAnimator = hit.collider.gameObject.GetComponent<Animator>();
				if (doorAnimator != null)
				{
					// Play the "Wood Door Open" animation
					doorAnimator.Play("Wood Door Open");

				}
			}


		}
	}

		void ReleaseObject()
	{
		if (grabbedObject != null)
		{
			// Re-enable physics by adding Rigidbody if missing
			if (grabbedObject.GetComponent<Rigidbody>() == null)
			{
				Rigidbody rb = grabbedObject.AddComponent<Rigidbody>();
				rb.mass = 1f;
				rb.drag = 0f;
				rb.angularDrag = 0.05f;
			}

			grabbedObject.transform.SetParent(null);
			grabbedObject.transform.rotation = originalRotation; // Restore original rotation

			grabbedObject = null;
			grabbedRb = null;
		}
	}

	void ThrowObject()
	{
		if (grabbedObject != null)
		{
			// Re-enable physics by adding Rigidbody if missing
			if (grabbedObject.GetComponent<Rigidbody>() == null)
			{
				Rigidbody rb = grabbedObject.AddComponent<Rigidbody>();
				rb.mass = 1f;
				rb.drag = 0f;
				rb.angularDrag = 0.05f;
			}

			Rigidbody objRb = grabbedObject.GetComponent<Rigidbody>();
			objRb.isKinematic = false; // Re-enable physics
			objRb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);

			grabbedObject.transform.SetParent(null);
			grabbedObject.transform.rotation = originalRotation; // Restore original rotation

			grabbedObject = null;
			grabbedRb = null;
		}
	}
}
