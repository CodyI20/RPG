using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Movement Events
    public event System.Action OnPlayerMoving;
    public event System.Action OnPlayerStoppedMoving;

    //Jump Events
    public event System.Action OnPlayerJumped;
    public event System.Action OnPlayerLanded;


    CharacterController characterController;

    [Header("Refernces")]
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
    private bool isGrounded = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        //Set the keys in the CustomInputManager
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

    private bool IsPressingMovementKeys()
    {
        return CustomInputManager.GetKey("MoveForward") ||
               CustomInputManager.GetKey("MoveBackward") ||
               CustomInputManager.GetKey("MoveLeft") ||
               CustomInputManager.GetKey("MoveRight") ||
               CustomInputManager.GetKeyDown("Jump");
    }

    private void Update()
    {
        CheckMovePlayer();
    }

    private void CheckMovePlayer()
    {
        bool groundedLastFrame = isGrounded;
        isGrounded = characterController.isGrounded;
        if(isGrounded && !groundedLastFrame)
        {
            OnPlayerLanded?.Invoke();
        }
        if(!IsPressingMovementKeys())
        {
            OnPlayerStoppedMoving?.Invoke();
            return;
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float horizontal = 0f;
        float vertical = 0f;

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

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        direction = transform.TransformDirection(direction); // Convert to local space

        if (direction.magnitude >= 0.1f)
        {
            OnPlayerMoving?.Invoke();
            RotateWithVelocity(direction);
            characterController.Move(direction * movementSpeed * Time.deltaTime);
        }
        else
        {
            OnPlayerStoppedMoving?.Invoke();
        }

        // Handle Jump
        if (isGrounded && CustomInputManager.GetKeyDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            OnPlayerJumped?.Invoke();
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void RotateWithVelocity(Vector3 velocity)
    {
        playerModel.LookAt(playerModel.position + velocity);
    }
}
