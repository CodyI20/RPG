using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeignDeath : MonoBehaviour
{
    EventBinding<FeignDeathEvent> feignDeathEvent;
    private void OnEnable()
    {
        feignDeathEvent = new EventBinding<FeignDeathEvent>(OnFeignDeath);
        EventBus<FeignDeathEvent>.Register(feignDeathEvent);
    }
    private void OnDisable()
    {
        EventBus<FeignDeathEvent>.Deregister(feignDeathEvent);
    }

    private void OnFeignDeath(FeignDeathEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Feign Death Event Raised in FeignDeath logic");
#endif
    }
}
