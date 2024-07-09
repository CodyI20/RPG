public class PlayerHealthBar : HealthbarUpdate
{
    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerHealthChanged -= UpdateHealthBar;
    }
}
