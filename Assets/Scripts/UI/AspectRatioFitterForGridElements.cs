using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum AspectType
{
    [InspectorName("Height Controls Width")]
    HeightControlsWidth,

    [InspectorName("Width Controls Height")]
    WidthControlsHeight
}

[RequireComponent(typeof(GridLayoutGroup))]
public class AspectRatioFitterForGridElements : MonoBehaviour
{
    /// <summary>
    /// Choose which aspect must fit with the parent size.
    /// </summary>
    [SerializeField, Tooltip("Choose which aspect must fit with the parent size.")]
    private AspectType _aspectType;

    /// <summary>
    /// The ratio (in percents) that the controlled aspect must respect.
    /// </summary>
    [SerializeField, Tooltip("The ratio (in percents) that the controlled size must respect.")]
    private float _ratio;

    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateAspect();
    }

    private void UpdateAspect()
    {
        Vector2 rectSize = _rectTransform.rect.size;

        switch (_aspectType)
        {
            case AspectType.HeightControlsWidth:
                {
                    int raw = _gridLayoutGroup.constraintCount;

                    // Calculate cell size
                    float cellsizeY = (rectSize.y / raw) - ((_gridLayoutGroup.spacing.y * (raw - 1)) / raw) - (_gridLayoutGroup.padding.vertical / raw);
                    float cellsizeX = cellsizeY * (_ratio / 100f);

                    // checks for the controlled aspect if it doesn't exceed the size of the parent with the number of children
                    int childrenNbr = transform.childCount;
                    int column = Mathf.CeilToInt(childrenNbr / (float)raw);

                    if ((cellsizeX * column) + (_gridLayoutGroup.spacing.x * (column - 1)) + (_gridLayoutGroup.padding.horizontal) > rectSize.x)
                    {
                        // Calculate new cell size
                        cellsizeX = (rectSize.x / column) - ((_gridLayoutGroup.spacing.x * (column - 1)) / column) - (_gridLayoutGroup.padding.horizontal / column);
                        cellsizeY = cellsizeX * (100f / _ratio);
                    }

                    _gridLayoutGroup.cellSize = new Vector2(cellsizeX, cellsizeY);
                    break;
                }

            case AspectType.WidthControlsHeight:
                {
                    int column = _gridLayoutGroup.constraintCount;

                    float cellsizeX = (rectSize.x / column) - ((_gridLayoutGroup.spacing.x * (column - 1)) / column) - (_gridLayoutGroup.padding.horizontal / column);
                    float cellsizeY = cellsizeX * (_ratio / 100f);

                    // checks for the controlled aspect if it doesn't exceed the size of the parent with the number of children
                    int childrenNbr = transform.childCount;
                    int raw = Mathf.CeilToInt(childrenNbr / (float)column);

                    if ((cellsizeY * raw) + (_gridLayoutGroup.spacing.y * (raw - 1)) + (_gridLayoutGroup.padding.vertical) > rectSize.y)
                    {
                        // Calculate new cell size
                        cellsizeY = (rectSize.y / raw) - ((_gridLayoutGroup.spacing.y * (raw - 1)) / raw) - (_gridLayoutGroup.padding.vertical/raw);
                        cellsizeX = cellsizeY * (100f / _ratio);
                    }

                    _gridLayoutGroup.cellSize = new Vector2(cellsizeX, cellsizeY);
                    break;
                }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();

        UpdateAspect();
    }
#endif
}
