using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MaintainAspectRatio : MonoBehaviour
{
    /// <summary>
    /// Aspect ratio for width based on the height.
    /// </summary>
    [SerializeField] 
    private float _aspectRatio = 1f;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Adjusts width based on height
        float height = _rectTransform.rect.height;
        _rectTransform.sizeDelta = new Vector2(height * _aspectRatio, height);
    }
}
