using UnityEngine;

public class SpiritWolfSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("Drag in the prefab of the spirit wolf")] private GameObject _spiritWolfPrefab;

    private EventBinding<SpiritWolfSpawnedEvent> spiritWolfSpawned;

    private void OnEnable()
    {
        spiritWolfSpawned = new EventBinding<SpiritWolfSpawnedEvent>(OnSpiritWolfSpawned);
        EventBus<SpiritWolfSpawnedEvent>.Register(spiritWolfSpawned);
    }
    private void OnDisable()
    {
        EventBus<SpiritWolfSpawnedEvent>.Deregister(spiritWolfSpawned);
    }

    private void OnSpiritWolfSpawned(SpiritWolfSpawnedEvent e)
    {
        Instantiate(_spiritWolfPrefab, transform.position, transform.rotation);
#if UNITY_EDITOR
        Debug.Log("Spirit Wolf Spawned from SpiritWolfLogic");
#endif
    }
}
