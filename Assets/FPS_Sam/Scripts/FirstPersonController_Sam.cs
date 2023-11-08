using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Sam Robichaud 2022
// NSCC-Truro
// Based on tutorial by (Comp - 3 Interactive)  * with modifications *
// further modifications by Chris Schnurr

public class FirstPersonController_Sam : MonoBehaviour
{
    public enum State
    {
        inactive,
        active
    }

    private State state = State.active;

    public GameObject projectile;
    public GameObject guide;
    public bool canMove { get; private set; } = true;
    private bool isRunning => canRun && Input.GetKey(runKey);
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    #region Settings

    [Header("Functional Settings")]
    [SerializeField] private bool canRun = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canSlideOnSlopes = true;
    [SerializeField] private bool canZoom = true;
  

    [Header("Controls")]
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    [Header("Move Settings")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float slopeSpeeed = 12f;

    [Header("Look Settings")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 70.0f;
    [SerializeField, Range(-180, 1)] private float lowerLookLimit = -70.0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 1.8f;
    [SerializeField] private float timeToCrouch = 0.15f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Settings")]
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.05f;
    [SerializeField] private float walkBobSpeed = 11.1f;
    [SerializeField] private float walkBobAmount = 0.065f;
    [SerializeField] private float runBobSpeed = 16f;
    [SerializeField] private float runBobAmount = 0.1f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Zoom Settings")]
    [SerializeField] private float timeToZoom = 0.2f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

 
  

    // Sliding Settings
    private Vector3 hitPointNormal;
    private bool isSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 5.0f))
            {
                hitPointNormal = slopeHit.normal;

                //prevents the player from jumping while sliding
                if (Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit)
                {
                    canJump = false;
                }
                else
                {
                    canJump = true;
                }
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else { return false; }
        }
    }



    #endregion

    private Camera playerCamera;
    private CharacterController characterController;
    
    private TextMeshPro gunHud;
    private int ammoInClip = 9;
    private int ammoReloadAmount = 9;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    private Transform projectileOrigin;
    private Transform guideOrigin;
    private static bool reloading = false;
    public static bool Reloading { get { return reloading; } }
    private static float reloadTimer = 0;
    public static float ReloadTimer { get { return reloadTimer; } }
    private static float reloadTimerMax = 2;
    public static float ReloadTimerMax { get { return reloadTimerMax; } }


    private static float playerHealth = 20;
    private static float playerMaxHealth = 20;
    private static float playerStamina = 2;
    public static float PlayerStamina { get { return playerStamina; } }
    private static float playerMaxStamina = 2;
    public static float PlayerMaxStamina { get { return playerMaxStamina; } }
    public static float PlayerMaxHealth { get { return playerMaxHealth; } }
    private bool regenerating = false;
    private static bool decaying = false;
    private float regenTimer = 3f;
    private float regenTimerReset = 3f;
    private static float decayTimer = 1f;
    public static float DecayTimer { get { return decayTimer; } set { decayTimer = value; } }
    private static float decayTimerReset = 1f;
    public static float DecayTimerReset { get { return decayTimerReset; } }





    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;

        gunHud = GetComponentInChildren<TextMeshPro>();
        gunHud.text = ammoInClip.ToString();

        GameObject projectileOriginGO = GameObject.Find("Player/Cam/Gun/ProjectileOrigin");
        GameObject guideGO = GameObject.Find("Player/GuidePos");
        projectileOrigin = projectileOriginGO.transform;
        guideOrigin = guideGO.transform;

        playerHealth = playerMaxHealth;
        state = State.active;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(!gameManager.paused)
        {
            if (canMove)
            {
                HandleMovementInput();
                HandleMouseLook(); // look into moving into Lateupdate if motion is jittery

                if (canJump) { HandleJump(); }
                if (canCrouch) { HandleCrouch(); }
                if (canUseHeadbob) { HandleHeadBob(); }
                if (canZoom) { HandleZoom(); }


                ApplyFinalMovement();
            }

            if (Input.GetKeyDown("escape")) gameManager.Pause();
            if (Input.GetMouseButtonDown(0) && !reloading) Fire();
            if (Input.GetKeyDown("g")) Instantiate(guide, guideOrigin.position, guideOrigin.rotation);
        }

        if(reloading)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= reloadTimerMax)
            {
                reloadTimer = 0;
                reloading = false;

                ammoInClip = ammoReloadAmount;
                gunHud.text = ammoInClip.ToString();
                screenManager.UpdateGunStatus("Ready");
            }
        }

        if(state == State.active)
        {
            if (playerHealth < playerMaxHealth && !decaying && !regenerating)
            {
                regenTimer -= Time.deltaTime;
                if (regenTimer <= 0)
                {
                    screenManager.UpdateGunStatus("Regenerating Health");
                    regenerating = true;
                    regenTimer = 1;
                }
            }

            if (regenerating)
            {
                if (playerHealth < playerMaxHealth)
                {
                    regenTimer -= Time.deltaTime;
                    if (regenTimer <= 0)
                    {
                        playerHealth += Time.deltaTime;
                    }
                }
                else if (playerHealth >= playerMaxHealth)
                {
                    screenManager.UpdateGunStatus("Ready");
                    regenerating = false;
                    playerHealth = playerMaxHealth;
                    regenTimer = regenTimerReset;
                }
            }

            if (decaying)
            {
                if (playerHealth > 0)
                {
                    decayTimer -= Time.deltaTime;
                    if (decayTimer <= 0)
                    {
                        screenManager.UpdateGunStatus("Corrosive Damage Detected");
                        playerHealth -= Time.deltaTime;
                    }
                }
                else
                {
                    screenManager.UpdateGunStatus("You are dead");
                    gameManager.LoseGame();
                    decaying = false;
                    playerHealth = 0;
                    decayTimer = decayTimerReset;
                    state = State.inactive;
                }
            }

            if (playerStamina <= 0) canRun = false;

            if (isRunning)
            {
                playerStamina -= Time.deltaTime;
                if (playerStamina < 0) playerStamina = 0;
            }

            if(!isRunning && playerStamina < playerMaxStamina)
            {
                playerStamina += Time.deltaTime;
                if (playerStamina > playerMaxStamina)
                {
                    playerStamina = playerMaxStamina;
                    canRun = true;
                }
                }
        }

        
    }

    private void LateUpdate()
    {

    }

    private void Fire()
    {
        if (ammoInClip > 0)
        {
            ammoInClip--;
            gunHud.text = ammoInClip.ToString();
            menuManager.SfxSource.Play();
            Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
        }
        else
        {
            Reload();
        }
    }

    private void Reload()
    {
        gunHud.text = "-";
        reloading = true;
        screenManager.UpdateGunStatus("Reloading");
    }

    public static void SetDecay()
    {
        decaying = true;
    }

    public static void RemoveDecay()
    {
        decaying = false;
        screenManager.UpdateGunStatus("Ready");
    }

    public static float GetPlayerHealth()
    {
        return playerHealth;
    }

    private void HandleMovementInput()
    {
        // Read inputs
        currentInput = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxis("Horizontal"));

        // normalizes input when 2 directions are pressed at the same time
        // TODO; find a more elegant solution to normalize, this is a bit of a hack method to normalize it estimates and is not 100% accurate.
        currentInput *= (currentInput.x != 0.0f && currentInput.y != 0.0f) ? 0.7071f : 1.0f;

        // Sets the required speed multiplier
        currentInput *= (isCrouching ? crouchSpeed : isRunning ? runSpeed : walkSpeed);

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        // Rotate camera up/down
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, lowerLookLimit, upperLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Rotate player left/right
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

    }

    private void HandleJump()
    {
        if (shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadBob()
    {
        // TODO: find a better headbob system that feels more natural.
        
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isRunning ? runBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isRunning ? runBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }

        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }
            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }



    

    private void ApplyFinalMovement()
    {
        // Apply gravity if the character controller is not grounded
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (characterController.velocity.y < - 1 && characterController.isGrounded)
            moveDirection.y = 0;


        // sliding
        if (canSlideOnSlopes && isSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeeed;
        }

        // applies movement based on all inputs
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1.0f))
        { yield break; }
        
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView; // capture reference to current FOV
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }






}
