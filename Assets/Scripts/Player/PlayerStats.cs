using UnityEngine;

//Since it's a single-player game, we can use a singleton pattern to manage the player's stats.
public class PlayerStats : Singleton<PlayerStats>
{
    //Events
    public event System.Action OnDamageTaken;
    public event System.Action OnPlayerHealed;
    public event System.Action OnPlayerDeath;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
    }

    private void Update()
    {
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
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
#if UNITY_EDITOR
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
#endif
        OnDamageTaken?.Invoke();
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        currentHealth = 0;
#if UNITY_EDITOR
        Debug.Log("Player died!");
#endif
    }
}
