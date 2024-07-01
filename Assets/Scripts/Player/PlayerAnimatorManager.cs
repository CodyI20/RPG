using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            Debug.LogError("PlayerAnimatorManager: No Animator found in children");
        }
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        playerMovement.OnPlayerMoved += HandlePlayerMoved;
        playerMovement.OnPlayerStoppedMoving += HandlePlayerStoppedMoving;
    }
    
    private void OnDisable()
    {
        playerMovement.OnPlayerMoved -= HandlePlayerMoved;
        playerMovement.OnPlayerStoppedMoving -= HandlePlayerStoppedMoving;
    }

    private void HandlePlayerMoved()
    {
        animator.SetBool("isRunning", true);
    }

    private void HandlePlayerStoppedMoving()
    {
        animator.SetBool("isRunning", false);
    }
}
