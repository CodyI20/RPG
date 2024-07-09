using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : SoundManager
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip playerDeathSound;

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
        PlayClip(playerDeathSound);
    }
}
