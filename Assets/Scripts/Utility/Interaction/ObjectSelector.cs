using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class takes care of selecting and highlighting objects in the scene.
/// 
/// **NOTE**
/// Use the usesOutline flag to enable or disable the use of the outline effect when selecting and deselecting objects.
/// </summary>
public class ObjectSelector : MonoBehaviour
{
    public static event System.Action<Transform, Transform> OnSelection;
    public static event System.Action<Transform, Transform> OnDeselection;

    [Header("Outline settings")]
    [SerializeField] private bool usesOutline = true;
    private const string OUTLINE_TAG = "Outlineable";
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField, Range(0, 10)] private float outlineWidth = 2.0f;

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    EventBinding<QuestInProgressPreviewEvent> QuestInProgressPreviewEvent;
    EventBinding<QuestPreviewExitEvent> QuestPreviewExitEvent;

    private void OnEnable()
    {
        QuestInProgressPreviewEvent = new EventBinding<QuestInProgressPreviewEvent>(HandleQuestInProgressPreview);
        EventBus<QuestInProgressPreviewEvent>.Register(QuestInProgressPreviewEvent);
        QuestPreviewExitEvent = new EventBinding<QuestPreviewExitEvent>(HandleQuestPreviewExit);
        EventBus<QuestPreviewExitEvent>.Register(QuestPreviewExitEvent);
    }
    private void OnDisable()
    {
        EventBus<QuestInProgressPreviewEvent>.Deregister(QuestInProgressPreviewEvent);
        EventBus<QuestPreviewExitEvent>.Deregister(QuestPreviewExitEvent);
    }

    private void HandleQuestInProgressPreview(QuestInProgressPreviewEvent e)
    {
        Deselect();
    }
    
    private void HandleQuestPreviewExit(QuestPreviewExitEvent e)
    {
        Deselect();
    }

    void Update()
    {
        if (!Cursor.visible) return;
        HandleHighlight();
        HandleSelection();
    }

    private void HandleHighlight()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (highlight != null)
        {
            var outline = highlight.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
            highlight = null;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag(OUTLINE_TAG) && highlight != selection)
            {
                var outline = highlight.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = outlineColor;
                    outline.OutlineWidth = outlineWidth;
                }
                outline.enabled = true;
            }
            else
            {
                highlight = null;
            }
        }
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight != null)
            {
                if (selection != null && selection != highlight)
                {
                    var outline = selection.GetComponent<Outline>();
                    if (outline != null) { outline.enabled = false; OnDeselection?.Invoke(transform, selection); }
                    selection = null;
                }
                selection = highlight;
                var highlightOutline = selection.GetComponent<Outline>();
                if (highlightOutline != null) { highlightOutline.enabled = true; OnSelection?.Invoke(transform, selection); }
                highlight = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Deselect();
    }

    private void Deselect()
    {
        if (selection != null)
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) { outline.enabled = false; OnDeselection?.Invoke(transform, selection); }
            selection = null;
        }
    }
}
