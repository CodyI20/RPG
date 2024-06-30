using UnityEngine;

public class SpiritWolfSpawner : MonoBehaviour
{
    //Public static events that other scripts can subscribe to.
    public static event System.Action OnSpiritWolfSpawned; // This event gets triggered when the player successfully spawns the spirit wolf.
    public static event System.Action OnSpiritWolfSpawnFailed; // This event gets triggered when the player is unsuccessful in spawning the spirit wolf.

    //Private fields that are serialized in the Unity editor.
    [SerializeField, Tooltip("Drag in the prefab of the spirit wolf")] private GameObject _spiritWolfPrefab;
    [SerializeField] private KeyCode _spawnKey = KeyCode.T;
    [SerializeField] private float _spawnRate = 1f;

    private float spawnCooldownRemaining = 0f;

    #region SpawnLogic
    private void SpawnSpiritWolf()
    {
#if UNITY_EDITOR
        Debug.Log("Spawning Spirit Wolf...");
#endif
        Instantiate(_spiritWolfPrefab, transform.position, transform.rotation);
        spawnCooldownRemaining = _spawnRate;
        OnSpiritWolfSpawned?.Invoke();
    }

    private bool CanSpawn()
    {
        if (spawnCooldownRemaining > 0)
        {
            spawnCooldownRemaining -= Time.deltaTime;
            return false;
        }
        return true;
    }

    private bool PressedKey()
    {
        return Input.GetKeyDown(_spawnKey);
    }
    #endregion

    private void Update()
    {
        bool canSpawn = CanSpawn();
        if (PressedKey())
        {
            if (canSpawn)
                SpawnSpiritWolf();
            else
            {
                OnSpiritWolfSpawnFailed?.Invoke();
#if UNITY_EDITOR
                Debug.Log($"You need to wait {spawnCooldownRemaining:F1} seconds before spawning another spirit wolf.");
#endif
            }
        }
    }
}
