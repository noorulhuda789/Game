using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace NavKeypad
{
	public class Keypad : MonoBehaviour
	{
		[Header("Events")]
		[SerializeField] private UnityEvent onAccessGranted;
		[SerializeField] private UnityEvent onAccessDenied;

		[Header("Combination Code (9 Numbers Max)")]
		[SerializeField] private int keypadCombo = 1847;

		public UnityEvent OnAccessGranted => onAccessGranted;
		public UnityEvent OnAccessDenied => onAccessDenied;

		[Header("Settings")]
		[SerializeField] private string accessGrantedText = "Granted";
		[SerializeField] private string accessDeniedText = "Denied";

		[Header("Visuals")]
		[SerializeField] private float displayResultTime = 1f;
		[Range(0, 5)]
		[SerializeField] private float screenIntensity = 2.5f;

		[Header("Colors")]
		[SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // Orangy
		[SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); // Red
		[SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); // Greenish

		[Header("SoundFx")]
		[SerializeField] private AudioClip buttonClickedSfx;
		[SerializeField] private AudioClip accessDeniedSfx;
		[SerializeField] private AudioClip accessGrantedSfx;

		[Header("Component References")]
		[SerializeField] private Renderer panelMesh;
		[SerializeField] private TMP_Text keypadDisplayText;
		[SerializeField] private AudioSource audioSource;

		[Header("Door References")]
		[SerializeField] private GameObject door;
		[SerializeField] private string doorOpenAnimationName = "Wood Door Open"; // Name of the door open animation

		private string currentInput;
		private bool displayingResult = false;
		private bool accessWasGranted = false;

		private void Awake()
		{
			ClearInput();
			panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
		}

		private void Update()
		{
			HandleKeyboardInput();
		}

		// Handles keyboard input for typing numbers and submitting the code
		private void HandleKeyboardInput()
		{
			if (displayingResult || accessWasGranted) return;

			for (int i = 0; i <= 9; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					AddInput(i.ToString());
				}
			}

			if (Input.GetKeyDown(KeyCode.Return)) // Enter key
			{
				CheckCombo();
			}

			if (Input.GetKeyDown(KeyCode.Backspace)) // Clear input
			{
				ClearInput();
			}
		}

		// Adds input to the current code
		public void AddInput(string input)
		{
			audioSource.PlayOneShot(buttonClickedSfx);
			if (currentInput != null && currentInput.Length == 4) // 4 max passcode size
			{
				return;
			}

			currentInput += input;
			keypadDisplayText.text = currentInput;
		}

		public void CheckCombo()
		{
			if (int.TryParse(currentInput, out var currentCombo))
			{
				bool granted = currentCombo == keypadCombo;
				Debug.Log(granted);
				if (!displayingResult)
				{
					StartCoroutine(DisplayResultRoutine(granted));
				}
			}
			else
			{
				Debug.LogWarning("Couldn't process input for some reason.");
			}
		}

		private IEnumerator DisplayResultRoutine(bool granted)
		{
			displayingResult = true;
			if (granted) AccessGranted();
			else AccessDenied();

			yield return new WaitForSeconds(displayResultTime);
			displayingResult = false;
			if (granted) yield break;
			ClearInput();
			panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
		}

		private void AccessDenied()
		{
			keypadDisplayText.text = accessDeniedText;
			onAccessDenied?.Invoke();
			panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
			audioSource.PlayOneShot(accessDeniedSfx);
		}

		private void ClearInput()
		{
			currentInput = "";
			keypadDisplayText.text = currentInput;
		}

		private void AccessGranted()
		{
			accessWasGranted = true;
			keypadDisplayText.text = accessGrantedText;
			onAccessGranted?.Invoke();
			panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
			audioSource.PlayOneShot(accessGrantedSfx);

			// Trigger the door opening animation for the assigned door
			if (door != null)
			{
				Animator animator = door.GetComponent<Animator>();
				if (animator != null)
				{
					animator.Play(doorOpenAnimationName);
				}
				else
				{
					Debug.LogWarning($"Animator not found on the door: {door.name}");
				}
			}
			else
			{
				Debug.LogWarning("No door assigned to this keypad!");
			}
		}
	}
}
