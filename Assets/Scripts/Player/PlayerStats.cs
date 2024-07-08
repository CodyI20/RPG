using UnityEngine;
using UnityUtils;

//Since it's a single-player game, we can use a singleton pattern to manage the player's stats.
public class PlayerStats : Singleton<PlayerStats>
{
    //Events
    public event System.Action OnDamageTaken;
    public event System.Action OnPlayerHealed;
    public event System.Action OnPlayerDeath;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private EventBinding<HealEvent> healActioned;

    private void OnEnable()
    {
        healActioned = new EventBinding<HealEvent>(HandlePlayerHealed);
        EventBus<HealEvent>.Register(healActioned);
    }
    private void OnDisable()
    {
        EventBus<HealEvent>.Deregister(healActioned);
    }

    private void HandlePlayerHealed(HealEvent e)
    {
        //Healing logic for the player
#if UNITY_EDITOR
        Debug.Log("Handling healing effect on player!");
#endif
        Heal((int)e.amount);
    }

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(currentHealth <= 0) return;
        //TESTING: Press "M" to take damage
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(currentHealth/2); // Take some damage but not enough to kill you
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            TakeDamage(currentHealth); // One shot
        }
    }


    //Test function for the animation events
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
#if UNITY_EDITOR
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
#endif
        OnDamageTaken?.Invoke();
        if(currentHealth <= 0)
        {
            Die(damage);
        }
    }

    private void Heal(int amount)
    {
        if(currentHealth <= 0) return;
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
#if UNITY_EDITOR
        Debug.Log($"Healing the player for: {amount}; The new health value is: {currentHealth}");
#endif
    }

    private void Die(int amountOfDamageTaken)
    {
        OnPlayerDeath?.Invoke();
        currentHealth = 0;
#if UNITY_EDITOR
        Debug.Log($"Player died! He was dealt: {amountOfDamageTaken} damage.");
#endif
    }
}
