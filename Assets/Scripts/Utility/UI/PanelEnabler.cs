using UnityEngine;

public class PanelEnabler : MonoBehaviour
{
    [SerializeField] private GameObject MenuPanel;

    private void Awake()
    {
        MenuPanel.SetActive(false);
    }
    private void OnEnable()
    {
        PlayerStats.Instance.OnPlayerDeath += () => MenuPanel.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnPlayerDeath -= () => MenuPanel.SetActive(true);
    }

    public void ToggleMenu()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    //private void LateUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        MenuPanel.SetActive(!MenuPanel.activeSelf);
    //    }
    //}
}
