using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCPathing : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTarget;

    [Header("NPC Pathing Settings")]
    [SerializeField] private NPCHostilityType hostilityType = NPCHostilityType.Aggressive;
    [SerializeField] private Outline _outline;
    [SerializeField, Tooltip("This is the area the target has to be in for the chase to trigger")] private float detectionArea = 5f;
    [SerializeField, Tooltip("This is the area the target has to get out of for the chase to stop")] private float stopChaseArea = 10f;
    [SerializeField, Tooltip("The distance the NPC will stop from the player")] private float stoppingDistance = 2f;

    public float StoppingDistance => stoppingDistance;

    [Space(5)]
    [Header("Other settings")]
    [SerializeField, Range(0, 50f)] private float agentSpeed = 5f;

    private bool playerInRange = false;
    private bool playerDead = false;
    private Vector3 positionBeforeChaseBegan;
    private bool isEvading = false;

    private GameObject areaDetectionPoint;

    private bool IsAlive = true;

    EventBinding<NPCDeathEvent> npcDeathEventBinding;
    EventBinding<NPCTriggerCombatEvent> npcTriggerCombatBinding;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeed;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        agent.stoppingDistance = stoppingDistance;
        areaDetectionPoint = new GameObject($"{gameObject.name}DetectionPoint");
        areaDetectionPoint.transform.position = transform.position;
        switch (hostilityType)
        {
            case NPCHostilityType.Aggressive:
                _outline.OutlineColor = Color.red;
                break;
            case NPCHostilityType.Passive:
                _outline.OutlineColor = Color.yellow;
                break;
        }
    }

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += HandlePlayerDeath;
        npcDeathEventBinding = new EventBinding<NPCDeathEvent>(HandleNPCDeath);
        EventBus<NPCDeathEvent>.Register(npcDeathEventBinding);
        npcTriggerCombatBinding = new EventBinding<NPCTriggerCombatEvent>(HandleNPCTriggerCombat);
        EventBus<NPCTriggerCombatEvent>.Register(npcTriggerCombatBinding);
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerDeath -= HandlePlayerDeath;
        EventBus<NPCDeathEvent>.Deregister(npcDeathEventBinding);
        EventBus<NPCTriggerCombatEvent>.Deregister(npcTriggerCombatBinding);
        if (areaDetectionPoint != null)
        {
            Destroy(areaDetectionPoint);
        }
    }

    private void HandleNPCTriggerCombat(NPCTriggerCombatEvent e)
    {
        if (e.npcObject != gameObject) return;
        PlayerInRangeActions();
    }

    private void HandleNPCDeath(NPCDeathEvent e)
    {
        if (e.npcObject != gameObject) return;
        IsAlive = false;
    }

    private void HandlePlayerDeath()
    {
        playerDead = true;
    }

    private bool PlayerGotInRange()
    {
        return Vector3.Distance(areaDetectionPoint.transform.position, playerTarget.position) <= detectionArea;
    }

    private bool IsPlayerOutOfRange()
    {
        return Vector3.Distance(areaDetectionPoint.transform.position, playerTarget.position) > stopChaseArea;
    }

    private void Update()
    {
        CheckForHealthStatus();
        CheckForLogicOutput();
        HandleDestinationReached();
    }

    private void PlayerInRangeActions()
    {
        playerInRange = true;
        positionBeforeChaseBegan = transform.position;
    }

    private void CheckForHealthStatus()
    {
        if (!IsAlive)
        {
            playerInRange = false;
            agent.ResetPath();
            agent.isStopped = true;
        }
    }

    private void CheckForLogicOutput()
    {
        if (isEvading) return;

        if (playerDead)
        {
            ReturnToInitialPosition();
            return;
        }

        if (playerInRange)
        {
            FollowPlayer();
        }

        if (PlayerGotInRange())
        {
            if (!playerInRange)
            {
                PlayerInRangeActions();
            }
        }
        else if (IsPlayerOutOfRange())
        {
            if (playerInRange)
            {
                playerInRange = false;
                ReturnToInitialPosition();
            }
        }
    }

    private void MoveAI(Vector3 destination)
    {
        EventBus<NPCRunEvent>.Raise(new NPCRunEvent() { npcObject = gameObject });
        agent.SetDestination(destination);
    }

    private void FollowPlayer()
    {
        MoveAI(playerTarget.position);
    }

    private void ReturnToInitialPosition()
    {
        isEvading = true;
        MoveAI(positionBeforeChaseBegan);
    }

    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= stoppingDistance;
    }

    private void HandleDestinationReached()
    {
        if (HasReachedDestination())
        {
            if (isEvading)
            {
                isEvading = false;
                EventBus<NPCEvadeFinishedEvent>.Raise(new NPCEvadeFinishedEvent() { npcObject = gameObject });
                areaDetectionPoint.transform.position = transform.position;
            }
            EventBus<NPCIdleEvent>.Raise(new NPCIdleEvent() { npcObject = gameObject });
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (areaDetectionPoint == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionArea);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stopChaseArea);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(areaDetectionPoint.transform.position, detectionArea);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(areaDetectionPoint.transform.position, stopChaseArea);
        }
    }
}
