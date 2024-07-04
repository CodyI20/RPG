using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] AbilityView view;
    [SerializeField] AbilityData[] startingAbilities;
    AbilityController controller;

    bool isPlayerDead = false;

    void Awake()
    {
        controller = new AbilityController.Builder()
            .WithAbilities(startingAbilities)
            .Build(view);
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        isPlayerDead = true;
    }

    void Update()
    {
        if (!isPlayerDead) controller.Update(Time.deltaTime);
    }
}