using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public event System.Action OnPlayerMoved;
    public event System.Action OnPlayerStoppedMoving;
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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        //Set the keys in the CustomInputManager
        CustomInputManager.SetKey("MoveForward", moveForwardKey);
        CustomInputManager.SetKey("MoveBackward", moveBackwardKey);
        CustomInputManager.SetKey("MoveLeft", moveLeftKey);
        CustomInputManager.SetKey("MoveRight", moveRightKey);
        CustomInputManager.SetKey("Jump", jumpKey);
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
        if(!IsPressingMovementKeys())
        {
            OnPlayerStoppedMoving?.Invoke();
            return;
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
        //Swap to local space
        direction = transform.TransformDirection(direction);
        //Apply to velocity
        Vector3 velocity = direction * movementSpeed;

        RotateWithVelocity(velocity);

        characterController.Move(velocity * Time.deltaTime);

        //If the player moved, invoke the event
        OnPlayerMoved?.Invoke();
    }

    private void RotateWithVelocity(Vector3 velocity)
    {
        playerModel.LookAt(playerModel.position + velocity);
    }
}
