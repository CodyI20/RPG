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

    [Space(5)]
    [Header("Other settings")]
    [SerializeField, Range(0, 50f)] private float agentSpeed = 5f;

    private bool playerInRange = false;
    private Vector3 positionBeforeChaseBegan;
    private bool isEvading = false;

    private GameObject areaDetectionPoint;

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
        CheckForLogicOutput();
        HandleDestinationReached();
    }

    private void CheckForLogicOutput()
    {
        if (isEvading) return;

        if (playerInRange)
        {
            FollowPlayer();
        }

        if (PlayerGotInRange())
        {
            if (!playerInRange)
            {
                playerInRange = true;
                positionBeforeChaseBegan = transform.position;
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

    private void FollowPlayer()
    {
        agent.SetDestination(playerTarget.position);
    }

    private void ReturnToInitialPosition()
    {
        isEvading = true;
        agent.SetDestination(positionBeforeChaseBegan);
    }

    private bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= 0.05f;
    }

    private void HandleDestinationReached()
    {
        if (isEvading && HasReachedDestination())
        {
            isEvading = false;
            areaDetectionPoint.transform.position = transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (areaDetectionPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(areaDetectionPoint.transform.position, detectionArea);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(areaDetectionPoint.transform.position, stopChaseArea);
    }

    private void OnDisable()
    {
        if (areaDetectionPoint != null)
        {
            Destroy(areaDetectionPoint);
        }
    }
}
