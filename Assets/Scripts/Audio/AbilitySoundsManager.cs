using UnityEngine;

public class AbilitySoundsManager : SoundManager
{
    [Header("Ability sounds")]
    [SerializeField] private AudioClip[] spiritWolfClips;
    [SerializeField] private AudioClip feignDeathClip;
    [SerializeField] private AudioClip healClip;

    //Event bus bindings
    EventBinding<SpiritWolfSpawnedEvent> spiritWolfSpawned;
    EventBinding<FeignDeathEvent> feignDeath;
    EventBinding<HealVFXEvent> heal;

    private void OnEnable()
    {
        spiritWolfSpawned = new EventBinding<SpiritWolfSpawnedEvent>(HandleSpiritWolfSpawned);
        EventBus<SpiritWolfSpawnedEvent>.Register(spiritWolfSpawned);

        feignDeath = new EventBinding<FeignDeathEvent>(HandleFeignDeath);
        EventBus<FeignDeathEvent>.Register(feignDeath);

        heal = new EventBinding<HealVFXEvent>(HandleHeal);
        EventBus<HealVFXEvent>.Register(heal);
    }

    private void OnDisable()
    {
        EventBus<SpiritWolfSpawnedEvent>.Deregister(spiritWolfSpawned);
        EventBus<FeignDeathEvent>.Deregister(feignDeath);
        EventBus<HealVFXEvent>.Deregister(heal);
    }

    private void HandleSpiritWolfSpawned(SpiritWolfSpawnedEvent e)
    {
        PlayClip(PickRandomAudioClip(spiritWolfClips));
    }

    private void HandleFeignDeath(FeignDeathEvent e)
    {
        PlayClip(feignDeathClip);
    }

    private void HandleHeal(HealVFXEvent e)
    {
        PlayClip(healClip);
    }
}
