using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class HealVFXTrigger : MonoBehaviour
{
    [SerializeField] private float timeToWait = 0.3f;
    private EventBinding<HealEvent> _healEventBinding;
    private VisualEffect vfx;

    private void OnEnable()
    {
        _healEventBinding = new EventBinding<HealEvent>(OnHealEvent);
        EventBus<HealEvent>.Register(_healEventBinding);
    }
    private void OnDisable()
    {
        EventBus<HealEvent>.Deregister(_healEventBinding);
    }

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }

    private void OnHealEvent(HealEvent e)
    {
        StartCoroutine(PlayerAfterTime());
#if UNITY_EDITOR
        Debug.Log("Triggering the VFX for the heal ability");
#endif
    }

    IEnumerator PlayerAfterTime()
    {
        yield return new WaitForSeconds(timeToWait);
        EventBus<HealVFXEvent>.Raise(new HealVFXEvent());
        vfx.Play();
    }
}
