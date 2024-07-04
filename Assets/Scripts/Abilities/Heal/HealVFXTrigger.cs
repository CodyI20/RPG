using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class HealVFXTrigger : MonoBehaviour
{
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
        vfx.Play();
#if UNITY_EDITOR
        Debug.Log("Triggering the VFX for the heal ability");
#endif
    }
}
