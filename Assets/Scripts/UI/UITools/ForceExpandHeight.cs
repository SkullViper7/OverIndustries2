using UnityEngine;

[ExecuteInEditMode]
public class ForceExpandHeight : MonoBehaviour
{
    private RectTransform _rectTransform;

    private RectTransform _rectTransformParent;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransformParent = transform.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rectTransformParent.rect.height);
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, - _rectTransformParent.rect.height / 2);
    }
}
