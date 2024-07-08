using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(CanvasGroup))]
public class WarningMessages : MonoBehaviour
{
    //REFERENCES
    TextMeshProUGUI warningText;
    CanvasGroup canvasGroup;

    [Header("General warning message settings")]
    [SerializeField, Tooltip("How long will the warning text stay on the screen")] private float textDuration = 2f;
    [SerializeField, Tooltip("How long will the warning text take to fade out")] private float fadeDuration = 1f;

    //OUT OF RANGE MESSAGE
    [Space(10)]
    [SerializeField] private string outOfRangeMessage = "You need to get closer to interact!";
    private bool isHandlingOutOfRangeMessage;
    EventBinding<NPCInteractOutOfRangeEvent> warningInteractOutOfRangeEvent;

    private void Awake()
    {
        warningText = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        isHandlingOutOfRangeMessage = false;
    }

    private void OnEnable()
    {
        warningInteractOutOfRangeEvent = new EventBinding<NPCInteractOutOfRangeEvent>(HandleInteractOutOfRange);
        EventBus<NPCInteractOutOfRangeEvent>.Register(warningInteractOutOfRangeEvent);
    }

    private void OnDisable()
    {
        EventBus<NPCInteractOutOfRangeEvent>.Deregister(warningInteractOutOfRangeEvent);
    }

    private void HandleInteractOutOfRange(NPCInteractOutOfRangeEvent e)
    {
        if (isHandlingOutOfRangeMessage) return;
        isHandlingOutOfRangeMessage = true;
        canvasGroup.alpha = 1;
        warningText.text = outOfRangeMessage;
        StartCoroutine(FadeTextOut());
    }

    IEnumerator FadeTextOut()
    {
        yield return new WaitForSeconds(textDuration);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
#if UNITY_EDITOR
        Debug.Log("Done with the coroutine.......................");
#endif
        isHandlingOutOfRangeMessage = false;
    }

}
