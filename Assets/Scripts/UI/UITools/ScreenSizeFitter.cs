using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class ScreenSizeFitter : MonoBehaviour
{
    /// <summary>
    /// Choose which aspect must fit with the screen.
    /// </summary>
    [SerializeField, Tooltip("Choose which aspect must fit with the screen.")]
    private AspectFitter _aspectFitter;

    /// <summary>
    /// The maximum size that the aspect choosed can reach.
    /// </summary>
    [SerializeField, Tooltip("Choose which aspect must fit with the screen.")]
    private float _maxSize;

    /// <summary>
    /// The minimum anchor ratio that the object must adopt if the screen size is enough big to have a fixed size.
    /// </summary>
    [SerializeField, Tooltip("The minimum anchor ratio that the object must adopt if the screen size is enough big to have a fixed size.")]
    private Vector2 _defaultMinAnchorRatio;

    /// <summary>
    /// The maximum anchor ratio that the object must adopt if the screen size is enough big to have a fixed size.
    /// </summary>
    [SerializeField, Tooltip("The maximum anchor ratio that the object must adopt if the screen size is enough big to have a fixed size.")]
    private Vector2 _defaultMaxAnchorRatio;

    /// <summary>
    /// The minimum anchor ratio that the object must adopt if the screen size is too small.
    /// </summary>
    [SerializeField, Tooltip("The minimum anchor ratio that the object must adopt if the screen size is too small.")]
    private Vector2 _fittMinAnchorRatio;

    /// <summary>
    /// The maximum anchor ratio that the object must adopt if the screen size is too small.
    /// </summary>
    [SerializeField, Tooltip("The maximum anchor ratio that the object must adopt if the screen size is too small.")]
    private Vector2 _fittMaxAnchorRatio;

    [SerializeField]
    private RectTransform _canvasRectTransform;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void FitSize()
    {
        switch (_aspectFitter)
        {
            case AspectFitter.HeightFitter:
                float screenHeight = _canvasRectTransform.rect.size.y;
                float effectiveHeight = screenHeight * (_fittMaxAnchorRatio.y - _fittMinAnchorRatio.y);

                // If effective height is lower than the minimum size, object height must fit with anchor ratio
                if (effectiveHeight < _maxSize)
                {
                    _rectTransform.anchorMin = _fittMinAnchorRatio;
                    _rectTransform.anchorMax = _fittMaxAnchorRatio;

                    _rectTransform.offsetMin = Vector2.zero;
                    _rectTransform.offsetMax = Vector2.zero;
                }
                // Else object height is equal to the minimum size
                else
                {
                    _rectTransform.anchorMin = _defaultMinAnchorRatio;
                    _rectTransform.anchorMax = _defaultMaxAnchorRatio;

                    _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _maxSize);

                    _rectTransform.anchoredPosition = Vector2.zero;
                }

                break;

            case AspectFitter.WidthFitter:
                float screenWidth = _canvasRectTransform.rect.size.x;
                float effectiveWidth = screenWidth * (_fittMaxAnchorRatio.x - _fittMinAnchorRatio.x);

                // If effective width is lower than the minimum size, object height must fit with anchor ratio
                if (effectiveWidth < _maxSize)
                {
                    _rectTransform.anchorMin = _fittMinAnchorRatio;
                    _rectTransform.anchorMax = _fittMaxAnchorRatio;

                    _rectTransform.offsetMin = Vector2.zero;
                    _rectTransform.offsetMax = Vector2.zero;
                }
                // Else object width is equal to the minimum size
                else
                {
                    _rectTransform.anchorMin = _defaultMinAnchorRatio;
                    _rectTransform.anchorMax = _defaultMaxAnchorRatio;

                    _rectTransform.sizeDelta = new Vector2(_maxSize, _rectTransform.sizeDelta.y);

                    _rectTransform.anchoredPosition = Vector2.zero;
                }

                break;
        }
    }


    private void Update()
    {
        FitSize();
    }
}