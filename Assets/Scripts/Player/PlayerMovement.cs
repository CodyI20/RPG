using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Movement Events
    public event System.Action OnPlayerMovingFront;
    public event System.Action OnPlayerStoppedMoving;
    public event System.Action OnPlayerMovingBack;
    public static event System.Action<GameObject> OnPlayerAttemptingMove;

    //Jump Events
    public event System.Action OnPlayerJumped;
    public event System.Action OnPlayerFalling;
    public event System.Action OnPlayerLanded;

    CharacterController characterController;

    [Header("References")]
    [Space(10)]
    [SerializeField] private Transform playerModel;

    [Header("Movement Keys")]
    [SerializeField] private KeyCode moveForwardKey = KeyCode.W; //Default value is "W"
    [SerializeField] private KeyCode moveBackwardKey = KeyCode.S; //Default value is "S"
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A; //Default value is "A"
    [SerializeField] private KeyCode moveRightKey = KeyCode.D; //Default value is "D"
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; //Default value is "Space"

    [Header("Mouse Settings")]
    [SerializeField] private MouseButton freeRunButton = MouseButton.Forward;

    [Space(10)]
    [Header("Player settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField, Tooltip("This movement speed only affects the backwards movement"), Range(0, 1)] private float backwardsMovementSpeedPercentage = 0.5f; // Default value: 0.5f (Half)
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    private float gravity = 0f;

    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    private Vector3 horizontalVelocity = Vector3.zero;

    private bool playerIsDead = false;
    private bool isFreeRunning = false;

    private bool firstFrame;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Set the keys in the CustomInputManager
        CustomInputManager.SetKey("MoveForward", moveForwardKey);
        CustomInputManager.SetKey("MoveBackward", moveBackwardKey);
        CustomInputManager.SetKey("MoveLeft", moveLeftKey);
        CustomInputManager.SetKey("MoveRight", moveRightKey);
        CustomInputManager.SetKey("Jump", jumpKey);

        gravity = Physics.gravity.y;
#if UNITY_EDITOR
        Debug.Log("Gravity: " + gravity);
#endif
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += () => playerIsDead = true;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerDeath -= () => playerIsDead = true;
    }

    private void Start()
    {
        isGrounded = characterController.isGrounded;
        isFreeRunning = false;
        firstFrame = true;
    }

    private void Update()
    {
        CheckMovePlayer();
    }

    private bool IsPressingMovementKeys()
    {
        if(CustomInputManager.GetKey("MoveForward") || 
            CustomInputManager.GetKey("MoveBackward") || 
            CustomInputManager.GetKey("MoveLeft") ||
            CustomInputManager.GetKey("MoveRight"))
        {
            OnPlayerAttemptingMove?.Invoke(this.gameObject);
            return true;
        }
        return false;
    }

    private void CheckMovePlayer()
    {
        Vector3 directionVector = Vector3.zero;
        bool groundedLastFrame = isGrounded;
        isGrounded = characterController.isGrounded;

        if (!playerIsDead)
        {
            if (!firstFrame && isGrounded && !groundedLastFrame)
            {
                Debug.Log("Player Landed");
                OnPlayerLanded?.Invoke();
            }
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetMouseButtonDown((int)freeRunButton) && !isFreeRunning)
            {
                Debug.Log("Free Running Enabled");
                isFreeRunning = true;
            }
            else if (Input.GetMouseButtonDown((int)freeRunButton) || IsPressingMovementKeys() && isFreeRunning)
            {
                isFreeRunning = false;
            }

            if (isGrounded)
            {
                float horizontal;
                float vertical;


                if (isFreeRunning)
                {
                    horizontal = 0;
                    vertical = 1f;
                }
                else
                {
                    horizontal = 0;
                    vertical = 0;
                }


                if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
                {
                    vertical += 1f;
                }

                if (CustomInputManager.GetKey("MoveForward"))
                {
                    vertical += 1f;
                }
                if (CustomInputManager.GetKey("MoveBackward"))
                {
                    vertical -= 1f;
                }
                if (CustomInputManager.GetKey("MoveLeft"))
                {
                    horizontal -= 1f;
                }
                if (CustomInputManager.GetKey("MoveRight"))
                {
                    horizontal += 1f;
                }

                directionVector = new Vector3(horizontal, 0, vertical).normalized;
                directionVector = transform.TransformDirection(directionVector); // Convert to local space

                if (directionVector.magnitude >= 0.1f)
                {
                    if (vertical < 0)
                    {
                        OnPlayerStoppedMoving?.Invoke();
                        OnPlayerMovingBack?.Invoke();
                        horizontalVelocity = directionVector * movementSpeed * backwardsMovementSpeedPercentage; // Store the horizontal velocity
                        RotateWithVelocityBack(directionVector);
                    }
                    else
                    {
                        OnPlayerStoppedMoving?.Invoke();
                        OnPlayerMovingFront?.Invoke();
                        horizontalVelocity = directionVector * movementSpeed; // Store the horizontal velocity
                        RotateWithVelocityFront(directionVector);
                    }
                    OnPlayerAttemptingMove?.Invoke(this.gameObject);
                }
                else
                {
                    OnPlayerStoppedMoving?.Invoke();
                    horizontalVelocity = Vector3.zero; // Reset horizontal velocity if no movement keys are pressed
                }
            }

            // Handle Jump
            if (isGrounded && CustomInputManager.GetKeyDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                OnPlayerJumped?.Invoke();
            }
        }
        else
        {
            if (isGrounded)
            {
                horizontalVelocity = Vector3.zero;
            }
        }


        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        if (isGrounded && !groundedLastFrame)
        {
            Debug.Log("Player Landed");
            horizontalVelocity = Vector3.zero;
            OnPlayerLanded?.Invoke();
        }
        // Combine horizontal and vertical velocity
        Vector3 finalVelocity = horizontalVelocity + new Vector3(0, velocity.y, 0);
        if (!isGrounded && velocity.y < 0)
        {
            OnPlayerFalling?.Invoke();
        }

        characterController.Move(finalVelocity * Time.deltaTime);
        if(firstFrame)
        {
            firstFrame = false;
        }
    }

    private void RotateWithVelocityFront(Vector3 velocity)
    {
        if (velocity != Vector3.zero) // Ensure velocity is not zero to avoid invalid LookRotation
        {
            // Calculate the target rotation based on the velocity
            Quaternion targetRotation = Quaternion.LookRotation(velocity);

            // Smoothly interpolate towards the target rotation
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    private void RotateWithVelocityBack(Vector3 velocity)
    {
        if (velocity != Vector3.zero) // Ensure velocity is not zero to avoid invalid LookRotation
        {
            // Calculate the target rotation based on the velocity
            Quaternion targetRotation = Quaternion.LookRotation(-velocity);

            // Smoothly interpolate towards the target rotation
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
