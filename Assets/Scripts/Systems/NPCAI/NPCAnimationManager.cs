using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class NPCAnimationManager : MonoBehaviour
{
    Animator _animator;

    EventBinding<NPCAttackEvent> attackEventBinding;
    EventBinding<NPCDeathEvent> deathEventBinding;
    EventBinding<NPCRunEvent> runEventBinding;
    EventBinding<NPCIdleEvent> idleEventBinding;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        attackEventBinding = new EventBinding<NPCAttackEvent>(HandleAttackEvent);
        EventBus<NPCAttackEvent>.Register(attackEventBinding);
        deathEventBinding = new EventBinding<NPCDeathEvent>(HandleDeathEvent);
        EventBus<NPCDeathEvent>.Register(deathEventBinding);
        runEventBinding = new EventBinding<NPCRunEvent>(HandleRunEvent);
        EventBus<NPCRunEvent>.Register(runEventBinding);
        idleEventBinding = new EventBinding<NPCIdleEvent>(HandleIdleEvent);
        EventBus<NPCIdleEvent>.Register(idleEventBinding);
    }

    private void OnDisable()
    {
        EventBus<NPCAttackEvent>.Deregister(attackEventBinding);
        EventBus<NPCDeathEvent>.Deregister(deathEventBinding);
        EventBus<NPCRunEvent>.Deregister(runEventBinding);
        EventBus<NPCIdleEvent>.Deregister(idleEventBinding);
    }

    #region Idle
    private void HandleIdleEvent(NPCIdleEvent e)
    {
        if (e.npcObject != gameObject) return;
        HandleIdleAnimation();
    }

    protected abstract void HandleIdleAnimation();
    #endregion

    #region Run
    private void HandleRunEvent(NPCRunEvent e)
    {
        if(e.npcObject != gameObject) return;
        HandleRunAnimation();
    }

    protected abstract void HandleRunAnimation();
    #endregion

    #region Attack
    private void HandleAttackEvent(NPCAttackEvent e)
    {
        if (e.npcObject != gameObject) return;
        HandleAttackAnimation();
    }

    protected abstract void HandleAttackAnimation();
    #endregion

    #region Death
    private void HandleDeathEvent(NPCDeathEvent e)
    {
        if (e.npcStats.gameObject != gameObject) return;
#if UNITY_EDITOR
        Debug.Log($"{e.npcStats.gameObject} is the same as {gameObject}! Horraaay!");
#endif
        HandleDeathAnimation();
    }

    protected abstract void HandleDeathAnimation();
    #endregion

    #region Animation Setters
    protected void SetBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    protected void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    protected void SetInteger(string name, int value)
    {
        _animator.SetInteger(name, value);
    }

    protected void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }
    #endregion
}
