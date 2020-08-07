using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectStretcher : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect = default;

    private VerticalLayoutGroup vlg = default;
    private RectTransform rectTransform = default;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        if (vlg == null) {
            vlg = GetComponent<VerticalLayoutGroup>();
        }

        if (rectTransform == null) {
            rectTransform = GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        UpdateHeight();
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateHeight();
    }

    [ContextMenu("Update Height")]
    public void UpdateHeight()
    {
        GetComponents();

        float height = 0f;
        for(int i = 0; i < transform.childCount; i++) {
            RectTransform childRectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            height += childRectTransform.rect.height;
            if (i < transform.childCount - 1) {
                height += vlg.spacing;
            }
        }
        height += vlg.padding.top + vlg.padding.bottom;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
