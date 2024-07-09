using UnityEngine;

public class PanelEnabler : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;

    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += () => MenuPanel.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerDeath -= () => MenuPanel.SetActive(true);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuPanel.SetActive(!MenuPanel.activeSelf);
        }
    }
}
