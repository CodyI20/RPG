using System.Collections;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Other settings")]
    [SerializeField] private float timeToDestroy = 10f;
    [SerializeField] private float spawnDelay = 11f;
    [SerializeField, Tooltip("Pass in the prefab used to spawn this game object")] private GameObject prefab;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    EventBinding<NPCEvadeFinishedEvent> evadeFinishedBinding;
    EventBinding<NPCEvasionEvent> evasionEventBinding;

    public bool IsDead => currentHealth <= 0;
    public float CurrentHealthPercentage => currentHealth / maxHealth;
    private bool isEvading = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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
        EventBus<NPCHealthChangeEvent>.Raise(new NPCHealthChangeEvent() { npcObject = gameObject, currentHealthPercentage = currentHealth/maxHealth});
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
        EventBus<NPCHealthChangeEvent>.Raise(new NPCHealthChangeEvent() { npcObject = gameObject, currentHealthPercentage = 1 });
    }

    private void HandleEvasionEvent(NPCEvasionEvent e)
    {
        if (e.npcObject != gameObject) return;
        isEvading = true;
    }

    private void Die()
    {
        EventBus<NPCDeathEvent>.Raise(new NPCDeathEvent() { npcObject = gameObject });
        EventBus<NPCHealthChangeEvent>.Raise(new NPCHealthChangeEvent() { npcObject = gameObject, currentHealthPercentage = 0 });
        //GameObjectSpawner.Instance.SpawnObjectAfterDelay(prefab, initialPosition, initialRotation, spawnDelay);
        Destroy(gameObject, timeToDestroy);
    }
}
