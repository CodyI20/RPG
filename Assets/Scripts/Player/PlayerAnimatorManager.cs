using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private HashSet<string> activeAnimations = new HashSet<string>();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerAnimatorManager: No Animator found in children");
        }
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (animator)
        {
            HandleIdleAnimation();
        }
    }

    private void HandleIdleAnimation()
    {
        // If no other animations are active, set the idle animation
        if (activeAnimations.Count == 0)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }

    #region EventSubscriptions
    private void OnEnable()
    {
        playerMovement.OnPlayerMoving += () => SetAnimationState("Running", true);
        playerMovement.OnPlayerStoppedMoving += () => SetAnimationState("Running", false);
        playerMovement.OnPlayerJumped += () => SetAnimationTrigger("Jumped");
        playerMovement.OnPlayerLanded += () => SetAnimationTrigger("Landed");
    }

    private void OnDisable()
    {
        playerMovement.OnPlayerMoving -= () => SetAnimationState("Running", true);
        playerMovement.OnPlayerStoppedMoving -= () => SetAnimationState("Running", false);
        playerMovement.OnPlayerJumped -= () => SetAnimationTrigger("Jumped");
        playerMovement.OnPlayerLanded -= () => SetAnimationTrigger("Landed");
    }
    #endregion

    private void SetAnimationState(string animationName, bool state)
    {
        animator.SetBool(animationName, state);
        if (state)
        {
            activeAnimations.Add(animationName);
        }
        else
        {
            activeAnimations.Remove(animationName);
        }
    }

    private void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        // Assuming trigger animations are one-shot and should be removed immediately after setting
        activeAnimations.Add(triggerName);
        Invoke(nameof(ClearTriggerAnimation), 0.1f); // Slight delay to allow animation to register
    }

    private void ClearTriggerAnimation(string triggerName)
    {
        activeAnimations.Remove(triggerName);
    }
}
