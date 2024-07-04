using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    
    EventBinding<PlayerAnimationEvent> eventBinding;
    //private HashSet<string> activeAnimations = new HashSet<string>();

    //Constants for animation states to avoid typos and magic strings
    private const string RUNNING = "Running";
    private const string FALLING = "Falling";
    private const string JUMPING = "Jumping";
    private const string LANDED = "Landed";
    private const string DYING = "Dying";
    private const string MOVING_BACK = "MovingBack";
    private const string IDLE = "isIdle";

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerAnimatorManager: No Animator found in children");
        }
        playerMovement = GetComponent<PlayerMovement>();
    }

    //private void Update()
    //{
    //    if (animator != null)
    //    {
    //        HandleIdleAnimation();
    //    }
    //}

    //private void HandleIdleAnimation()
    //{
    //    // If no other animations are active, set the idle animation
    //    if (activeAnimations.Count == 0)
    //    {
    //        animator.SetBool("isIdle", true);
    //    }
    //    else
    //    {
    //        animator.SetBool("isIdle", false);
    //    }
    //}

    #region EventSubscriptions
    private void OnEnable()
    {
        eventBinding = new EventBinding<PlayerAnimationEvent>(HandlePlayerEvent);
        EventBus<PlayerAnimationEvent>.Register(eventBinding);
        playerMovement.OnPlayerMovingFront += () => SetAnimationState(RUNNING, true);
        playerMovement.OnPlayerStoppedMoving += () => SetAnimationState(RUNNING, false);
        playerMovement.OnPlayerFalling += () => SetAnimationState(FALLING, true);
        playerMovement.OnPlayerLanded += () => SetAnimationState(FALLING, false);
        playerMovement.OnPlayerJumped += () => SetAnimationState(JUMPING,true);
        playerMovement.OnPlayerFalling += () => SetAnimationState(JUMPING, false);
        playerMovement.OnPlayerLanded += () => SetAnimationTrigger(LANDED);
        playerMovement.OnPlayerMovingFront += () => ResetAnimationTrigger(LANDED);
        playerMovement.OnPlayerMovingBack += () => SetAnimationState(MOVING_BACK, true);
        playerMovement.OnPlayerStoppedMoving += () => SetAnimationState(MOVING_BACK, false);
        PlayerStats.Instance.OnPlayerDeath += () => SetAnimationTrigger(DYING);
    }

    private void OnDisable()
    {
        EventBus<PlayerAnimationEvent>.Deregister(eventBinding);
        playerMovement.OnPlayerMovingFront -= () => SetAnimationState(RUNNING, true);
        playerMovement.OnPlayerStoppedMoving -= () => SetAnimationState(RUNNING, false);
        playerMovement.OnPlayerFalling -= () => SetAnimationState(FALLING, true);
        playerMovement.OnPlayerLanded -= () => SetAnimationState(FALLING, false);
        playerMovement.OnPlayerJumped -= () => SetAnimationState(JUMPING, true);
        playerMovement.OnPlayerFalling -= () => SetAnimationState(JUMPING, false);
        playerMovement.OnPlayerLanded -= () => SetAnimationTrigger(LANDED);
        playerMovement.OnPlayerMovingFront -= () => ResetAnimationTrigger(LANDED);
        playerMovement.OnPlayerMovingBack -= () => SetAnimationState(MOVING_BACK, true);
        playerMovement.OnPlayerStoppedMoving -= () => SetAnimationState(MOVING_BACK, false);
        PlayerStats.Instance.OnPlayerDeath += () => SetAnimationTrigger(DYING);
    }
    #endregion

    private void HandlePlayerEvent(PlayerAnimationEvent playerEvent)
    {
        PlayAnimation(playerEvent.animationHash);
    }

    private void SetAnimationState(string animationName, bool state)
    {
        animator.SetBool(animationName, state);
        //if (state)
        //{
        //    activeAnimations.Add(animationName);
        //}
        //else
        //{
        //    activeAnimations.Remove(animationName);
        //}
    }

    private void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        // Assuming trigger animations are one-shot and should be removed immediately after setting
        //activeAnimations.Add(triggerName);
        //StartCoroutine(ClearTriggerAnimation(triggerName)); // Slight delay to allow animation to register
    }

    private void ResetAnimationTrigger(string triggerName)
    {
        animator.ResetTrigger(triggerName);
    }

    private void PlayAnimation(int animationHash)
    {
        animator.Play(animationHash);
    }

    //IEnumerator ClearTriggerAnimation(string triggerName)
    //{
    //    // Wait for the animation to finish
    //    AnimatorClipInfo animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
    //    yield return new WaitForSeconds(animator.GetAni);
    //    activeAnimations.Remove(triggerName);
    //    yield return null;
    //}
}
