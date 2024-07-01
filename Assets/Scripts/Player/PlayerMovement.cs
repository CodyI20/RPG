using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Movement Events
    public event System.Action OnPlayerMoving;
    public event System.Action OnPlayerStoppedMoving;

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

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Space(10)]
    [Header("Player settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    private float gravity = 0f;

    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    private Vector3 horizontalVelocity = Vector3.zero;

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

    private void Start()
    {
        isGrounded = characterController.isGrounded;
    }

    private bool IsPressingMovementKeys()
    {
        return CustomInputManager.GetKey("MoveForward") ||
               CustomInputManager.GetKey("MoveBackward") ||
               CustomInputManager.GetKey("MoveLeft") ||
               CustomInputManager.GetKey("MoveRight");
    }

    private void Update()
    {
        CheckMovePlayer();
    }

    private void CheckMovePlayer()
    {
        Vector3 directionVector = Vector3.zero;
        bool groundedLastFrame = isGrounded;
        isGrounded = characterController.isGrounded;

        if (isGrounded && !groundedLastFrame)
        {
            Debug.Log("Player Landed");
            OnPlayerLanded?.Invoke();
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isGrounded)
        {
            float horizontal = 0f;
            float vertical = 0f;
            
            if(Input.GetMouseButton(0) && Input.GetMouseButton(1))
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
                OnPlayerMoving?.Invoke();
                RotateWithVelocity(directionVector);
                horizontalVelocity = directionVector * movementSpeed; // Store the horizontal velocity
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

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal and vertical velocity
        Vector3 finalVelocity = horizontalVelocity + new Vector3(0, velocity.y, 0);
        if(!isGrounded && velocity.y < 0)
        {
            OnPlayerFalling?.Invoke();
        }

        characterController.Move(finalVelocity * Time.deltaTime);
    }

    private void RotateWithVelocity(Vector3 velocity)
    {
        playerModel.LookAt(playerModel.position + velocity);
    }
}
