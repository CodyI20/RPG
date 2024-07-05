using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineEnabler : MonoBehaviour
{
    public static event System.Action OnSelection;
    private const string OUTLINE_TAG = "Outlineable";

    [Header("Outline settings")]
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField, Range(0, 10)] private float outlineWidth = 2.0f;

    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

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
                selection = highlight;
                var highlightOutline = selection.GetComponent<Outline>();
                if (highlightOutline != null) { highlightOutline.enabled = true; OnSelection?.Invoke(); }
                highlight = null;
            }
            else
            {
                if (selection != null)
                {
                    var outline = selection.GetComponent<Outline>();
                    if (outline != null) outline.enabled = false;
                    selection = null;
                }
            }
        }

        if (selection != null && Input.GetKeyDown(KeyCode.Escape))
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
            selection = null;
        }
    }
}
