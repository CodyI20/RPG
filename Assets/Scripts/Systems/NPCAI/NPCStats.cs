using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Other settings")]
    [SerializeField] private float timeToDestroy = 10f;

    EventBinding<NPCEvadeFinishedEvent> evadeFinishedBinding;
    EventBinding<NPCEvasionEvent> evasionEventBinding;

    public bool IsDead => currentHealth <= 0;
    private bool isEvading = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        evadeFinishedBinding = new EventBinding<NPCEvadeFinishedEvent>(HandleEvadeFinished);
        EventBus<NPCEvadeFinishedEvent>.Register(evadeFinishedBinding);
        evasionEventBinding = new EventBinding<NPCEvasionEvent>(HandleEvasionEvent);
        EventBus<NPCEvasionEvent>.Register(evasionEventBinding);
    }

    private void OnDisable()
    {
        EventBus<NPCEvadeFinishedEvent>.Deregister(evadeFinishedBinding);
        EventBus<NPCEvasionEvent>.Deregister(evasionEventBinding);
    }

    public void TakeDamage(float damage)
    {
        if(isEvading) return;
        currentHealth -= damage;
        EventBus<NPCTriggerCombatEvent>.Raise(new NPCTriggerCombatEvent() { npcObject = gameObject });
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void HandleEvadeFinished(NPCEvadeFinishedEvent e)
    {
        if (e.npcObject != gameObject) return;
        isEvading = false;
        currentHealth = maxHealth;
    }

    private void HandleEvasionEvent(NPCEvasionEvent e)
    {
        if (e.npcObject != gameObject) return;
        isEvading = true;
    }

    private void Die()
    {
        EventBus<NPCDeathEvent>.Raise(new NPCDeathEvent() { npcObject = gameObject });
        Destroy(gameObject, timeToDestroy);
    }
}
