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

    EventBinding<NPCInteractInRangeEvent> InteractInRangeEventBinding;
    EventBinding<NPCExitInteractionOutOfRangeEvent> ExitInteractOutOfRangeBinding;
    EventBinding<NPCInteractOutOfRangeEvent> InteractOutOfRangeEventBinding;

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
        InteractInRangeEventBinding = new EventBinding<NPCInteractInRangeEvent>(HandleSelectionInRange);
        EventBus<NPCInteractInRangeEvent>.Register(InteractInRangeEventBinding);
        InteractOutOfRangeEventBinding = new EventBinding<NPCInteractOutOfRangeEvent>(HandleSelectionOutOfRange);
        EventBus<NPCInteractOutOfRangeEvent>.Register(InteractOutOfRangeEventBinding);
        ExitInteractOutOfRangeBinding = new EventBinding<NPCExitInteractionOutOfRangeEvent>(HandleDeselectionOutOfRange);
        EventBus<NPCExitInteractionOutOfRangeEvent>.Register(ExitInteractOutOfRangeBinding);
        ObjectSelector.OnDeselection += HandleDeselectionInRange;
    }

    private void OnDisable()
    {
        EventBus<NPCInteractInRangeEvent>.Deregister(InteractInRangeEventBinding);
        EventBus<NPCInteractOutOfRangeEvent>.Deregister(InteractOutOfRangeEventBinding);
        EventBus<NPCExitInteractionOutOfRangeEvent>.Deregister(ExitInteractOutOfRangeBinding);
        ObjectSelector.OnDeselection -= HandleDeselectionInRange;
    }

    private void HandleDeselectionInRange(Transform selector, Transform deselection)
    {
#if UNITY_EDITOR
        Debug.Log("Deselected: " + deselection.name);
#endif
        if (deselection == transform)
        {
            isSelected = false;
            //SOUND
            PlayRandomAudioClip(farewellAudioClips);
            //ROTATION
            HandleCoroutine(RotateToInitialPosition());
        }
    }

    private void HandleDeselectionOutOfRange(NPCExitInteractionOutOfRangeEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Deselected: " + e.selection.name + "but out of range!");
#endif
        if (e.selection == transform)
            isSelected = false;
    }

    private void HandleSelectionInRange(NPCInteractInRangeEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Selected: " + e.selection.name);
#endif
        if (e.selection == transform)
        {
            isSelected = true;
            //SOUND
            PlayRandomAudioClip(greetingsAudioClips);
            //ROTATION
            HandleCoroutine(ConstantlyRotateTowards(e.selector));
        }
    }

    private void HandleSelectionOutOfRange(NPCInteractOutOfRangeEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("Selected: " + e.selection.name + " but out of range!");
#endif
        if (e.selection == transform)
            isSelected = true;
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
