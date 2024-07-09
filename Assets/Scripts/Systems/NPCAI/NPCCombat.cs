using UnityEngine;

[RequireComponent(typeof(NPCPathing))]
[RequireComponent(typeof(NPCStats))]
public class NPCCombat : MonoBehaviour
{
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private int _attackDamage = 10;
    private float _attackRange;

    private float attackRateSave = 0f;

    Transform _playerTransform;
    NPCPathing _npcPathing;
    NPCStats _npcStats;

    private bool playerIsDead = false;

    private void Awake()
    {
        _npcPathing = GetComponent<NPCPathing>();
        _attackRange = _npcPathing.StoppingDistance;
        _npcStats = GetComponent<NPCStats>();
        attackRateSave = _attackRate;
        playerIsDead = false;
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += () => playerIsDead = true;
    }

    private void OnDestroy()
    {
        PlayerStats.Instance.OnPlayerDeath -= () => playerIsDead = true;
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_npcStats.IsDead || playerIsDead) return;
        TryAttackPlayer();
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(_playerTransform.position, transform.position) <= _attackRange;
    }

    private void TryAttackPlayer()
    {
        if(attackRateSave > 0)
        {
            attackRateSave -= Time.deltaTime;
        }else
        {
            attackRateSave = _attackRate;
            if (IsPlayerInRange())
            {
                EventBus<NPCAttackEvent>.Raise(new NPCAttackEvent() { npcObject = gameObject });
                PlayerStats.Instance.TakeDamage(_attackDamage);
            }
        }

    }
}
