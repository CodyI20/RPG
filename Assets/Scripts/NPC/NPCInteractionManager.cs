using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NPCInteractionManager : MonoBehaviour
{
    //AUDIO
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] greetingsAudioClips;
    [SerializeField] private AudioClip[] farewellAudioClips;

    //ROTATION
    [SerializeField] private Transform rotationTarget;
    private Quaternion initialRotation;
    private Coroutine rotationCoroutine;
    private bool isSelected = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (rotationTarget == null) Debug.LogError("Rotation target not set for: " + name);
    }

    private void Start()
    {
        initialRotation = rotationTarget.rotation;
    }

    private void OnEnable()
    {
        ObjectSelector.OnSelection += HandleSelection;
        ObjectSelector.OnDeselection += HandleDeselection;
    }

    private void OnDisable()
    {
        ObjectSelector.OnSelection -= HandleSelection;
        ObjectSelector.OnDeselection -= HandleDeselection;
    }

    private void HandleDeselection(Transform deselection)
    {
#if UNITY_EDITOR
        Debug.Log("Deselected: " + deselection.name);
#endif
        isSelected = false;
        if (deselection == transform)
        {
            //SOUND
            PlayRandomAudioClip(farewellAudioClips);
            //ROTATION
            HandleCoroutine(RotateToInitialPosition());
        }
    }

    private void HandleSelection(Transform selection)
    {
#if UNITY_EDITOR
        Debug.Log("Selected: " + selection.name);
#endif
        isSelected = true;
        if (selection == transform)
        {
            //SOUND
            PlayRandomAudioClip(greetingsAudioClips);
            //ROTATION
            HandleCoroutine(ConstantlyRotateTowards(Camera.main.transform));
        }
    }
    #region SOUND

    private void PlayRandomAudioClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return;
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], 1);
    }
    #endregion

    #region ROTATION

    private void HandleCoroutine(IEnumerator rotationAction)
    {
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(rotationAction);
    }

    private IEnumerator ConstantlyRotateTowards(Transform target)
    {
        while (isSelected)
        {
            Vector3 directionToTarget = target.position - rotationTarget.position;
            directionToTarget.y = 0; // Keep only the horizontal direction
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            rotationTarget.rotation = Quaternion.Slerp(rotationTarget.rotation, targetRotation, Time.deltaTime * 2); // Adjust speed as needed
            yield return null;
        }
    }

    private IEnumerator RotateToInitialPosition()
    {
        while (Quaternion.Angle(rotationTarget.rotation, initialRotation) > 0.01f)
        {
            rotationTarget.rotation = Quaternion.Slerp(rotationTarget.rotation, initialRotation, Time.deltaTime * 2); // Adjust speed as needed
            yield return null;
        }
        rotationTarget.rotation = initialRotation;
    }
    #endregion

}
