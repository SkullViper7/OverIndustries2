using UnityEngine;

public enum AspectFitter
{
    [InspectorName("Height Fitter")]
    HeightFitter,

    [InspectorName("Width Fitter")]
    WidthFitter
}

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class PopUpSizeFitter : MonoBehaviour
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
    private float _minSize;

    /// <summary>
    /// The anchor ratio that the pop up must adopt if the screen size is too small.
    /// </summary>
    [SerializeField, Tooltip("The anchor ratio that the pop up must adopt if the screen size is too small. (X for bottom and Y for top")]
    private Vector2 _anchorRatio;

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
                float effectiveHeight = screenHeight * (_anchorRatio.y - _anchorRatio.x);

                // If effective height is lower than the minimum size, pop-up height must fit with anchor ratio
                if (effectiveHeight < _minSize)
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, _anchorRatio.x);
                    _rectTransform.anchorMax = new Vector2(0.5f, _anchorRatio.y);

                    _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, 0);
                    _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, 0);
                }
                // Else pop-up height is equal to the minimum size
                else
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                    _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _minSize);

                    _rectTransform.localPosition = Vector2.zero;
                }

                break;

            case AspectFitter.WidthFitter:
                float screenWidth = _canvasRectTransform.rect.size.x;
                float effectiveWidth = screenWidth * (_anchorRatio.y - _anchorRatio.x);

                // If effective width is lower than the minimum size, pop-up width must fit with anchor ratio
                if (effectiveWidth < _minSize)
                {
                    _rectTransform.anchorMin = new Vector2(_anchorRatio.x, 0.5f);
                    _rectTransform.anchorMax = new Vector2(_anchorRatio.y, 0.5f);

                    _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
                    _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
                }
                // Else pop-up width is equal to the minimum size
                else
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                    _rectTransform.sizeDelta = new Vector2(_minSize, _rectTransform.sizeDelta.y);

                    _rectTransform.localPosition = Vector2.zero;
                }

                break;
        }
    }


    private void Update()
    {
        FitSize();
    }
}
