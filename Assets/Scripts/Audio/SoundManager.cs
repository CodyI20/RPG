using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    protected virtual void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, audioSource.volume);
    }

    protected AudioClip PickRandomAudioClip(AudioClip[] range)
    {
        return range[Random.Range(0, range.Length)];
    }
}
