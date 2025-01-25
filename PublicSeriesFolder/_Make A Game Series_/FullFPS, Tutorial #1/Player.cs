using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Basic Mechanics")]
    public bool canRun;
    public bool canJump;
    public bool canCrouch;
    public bool canProne;
    public bool useFootsteps;
    public bool useStamina; // New variable for using stamina
    [Space]
    [Header("Walking/Running")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    [Space]
    [Header("Jumping")]
    public float jumpPower = 7f;
    public float gravity = 10f;
    [Space]
    [Header("Crouching")]
    public float crouchSpeed = 3f;
    public float crouchHeight = 1f;
    [Space]
    [Header("Proning")]
    public float proneSpeed = 1.5f;
    public float proneHeight = 0.5f;
    [Space]
    [Header("Camera Settings")]
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    [Space]
    [Header("Footstep Settings")]
    public float walkDelay = 0.5f;
    public float runDelay = 0.3f;
    public float crouchDelay = 0.7f;
    public float proneDelay = 1.0f;
    public AudioClip[] grassFootstepSounds;
    public AudioClip[] dirtFootstepSounds;
    public AudioClip[] carpetFootstepSounds;
    public AudioClip[] tileFootstepSounds;
    public AudioClip[] woodFootstepSounds;
    [Space]
    [Header("Stamina Settings")]
    public Image staminaFill; // Reference to the stamina bar fill
    public float maxStamina = 100f;
    public float staminaRegenRate = 10f; // Rate at which stamina regenerates
    public float staminaDepletionRate = 20f; // Rate at which stamina depletes while running
    public float minJumpStamina = 20f; // Minimum stamina required to jump

    private bool isCrouching = false;
    private bool isProne = false;
    private float standardHeight;
    private Vector3 standardCameraPosition;
    private Vector3 targetCameraPosition;
    private Vector3 lastPosition;
    private float lastFootstepTime;
    private float currentFootstepDelay;
    private float currentStamina; // Current stamina level

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private bool canMove = true;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        standardHeight = characterController.height;
        standardCameraPosition = playerCamera.transform.localPosition;
        targetCameraPosition = standardCameraPosition;
        lastPosition = transform.position;
        lastFootstepTime = Time.time;
        currentFootstepDelay = walkDelay; // Default to walk delay
        currentStamina = maxStamina; // Initialize current stamina
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if(currentStamina > 1)
        {
            canRun = true;
        }

        else if(currentStamina < 1)
        {
            canRun = false;
            
        }


        bool isRunning = canRun && Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isProne; // Check canRun instead of just canRun variable

        float curSpeedX;
        float curSpeedY;

        if (canMove)
        {
            if (isProne)
            {
                curSpeedX = proneSpeed * Input.GetAxis("Vertical");
                curSpeedY = proneSpeed * Input.GetAxis("Horizontal");
                currentFootstepDelay = proneDelay;
            }
            else if (isCrouching)
            {
                curSpeedX = crouchSpeed * Input.GetAxis("Vertical");
                curSpeedY = crouchSpeed * Input.GetAxis("Horizontal");
                currentFootstepDelay = crouchDelay;
            }
            else
            {
                curSpeedX = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical");
                curSpeedY = (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal");
                currentFootstepDelay = isRunning ? runDelay : walkDelay;

                // Deduct stamina if running and useStamina is true
                if (useStamina && isRunning)
                {
                    currentStamina -= staminaDepletionRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                }
                else if (useStamina && !isRunning)
                {
                    // Regenerate stamina if not running and useStamina is true
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                }

                // Update the stamina bar fill image
                UpdateStaminaFill();
            }

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Check if we have enough stamina to jump
            bool canJumpCondition = canJump && !isProne && !isCrouching && Input.GetButton("Jump") && characterController.isGrounded && currentStamina >= minJumpStamina;

            if (canJumpCondition)
            {
                moveDirection.y = jumpPower;
                // Deduct stamina when jumping
                if (useStamina)
                {
                    currentStamina -= minJumpStamina;
                    currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                }
            }
            else
            {
                moveDirection.y = movementDirectionY - gravity * Time.deltaTime;
            }

            if (canCrouch && Input.GetKeyDown(KeyCode.C))
            {
                ToggleCrouch();
            }

            if (canProne && Input.GetKeyDown(KeyCode.Z))
            {
                ToggleProne();
            }

            characterController.Move(moveDirection * Time.deltaTime);
            HandleRotation();

            if (useFootsteps && (moveDirection != Vector3.zero))
            {
                CheckFootsteps();
            }
            else
            {
                StopFootsteps();
            }
        }
    }





    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            targetCameraPosition = new Vector3(playerCamera.transform.localPosition.x, standardCameraPosition.y - 0.5f, playerCamera.transform.localPosition.z);
            StartCoroutine(CrouchSmoothly(crouchHeight));
        }
        else
        {
            targetCameraPosition = standardCameraPosition;
            StartCoroutine(CrouchSmoothly(standardHeight));
        }

        isProne = false;
    }

    void ToggleProne()
    {
        isProne = !isProne;
        if (isProne)
        {
            targetCameraPosition = new Vector3(playerCamera.transform.localPosition.x, standardCameraPosition.y - 0.5f, playerCamera.transform.localPosition.z);
            StartCoroutine(CrouchSmoothly(proneHeight));
        }
        else
        {
            targetCameraPosition = standardCameraPosition;
            StartCoroutine(CrouchSmoothly(standardHeight));
        }

        isCrouching = false;
    }

    IEnumerator CrouchSmoothly(float targetHeight)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = playerCamera.transform.localPosition;
        float startHeight = characterController.height;

        while (elapsedTime < 0.25f)
        {
            playerCamera.transform.localPosition = Vector3.Lerp(startPosition, targetCameraPosition, elapsedTime / 0.25f);
            characterController.height = Mathf.Lerp(startHeight, targetHeight, elapsedTime / 0.25f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = targetCameraPosition;
        characterController.height = targetHeight;
    }

    void HandleRotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void CheckFootsteps()
    {
        if (Time.time - lastFootstepTime > currentFootstepDelay)
        {
            if (transform.position != lastPosition)
            {
                lastPosition = transform.position;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust the radius as needed

                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Grass"))
                    {
                        PlayRandomFootstep(grassFootstepSounds);
                    }
                    else if (collider.CompareTag("Dirt"))
                    {
                        PlayRandomFootstep(dirtFootstepSounds);
                    }
                    else if (collider.CompareTag("Carpet"))
                    {
                        PlayRandomFootstep(carpetFootstepSounds);
                    }
                    else if (collider.CompareTag("Tile"))
                    {
                        PlayRandomFootstep(tileFootstepSounds);
                    }
                    else if (collider.CompareTag("Wood"))
                    {
                        PlayRandomFootstep(woodFootstepSounds);
                    }
                }

                lastFootstepTime = Time.time;
            }
        }
    }

    void StopFootsteps()
    {
        // Stop all footsteps sounds
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
    }

    void PlayRandomFootstep(AudioClip[] footstepSounds)
    {
        if (footstepSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, footstepSounds.Length);
            AudioSource.PlayClipAtPoint(footstepSounds[randomIndex], transform.position);
        }
    }

    void UpdateStaminaFill()
    {
        if (staminaFill != null)
        {
            staminaFill.fillAmount = currentStamina / maxStamina;
        }
    }
}
