using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ExpansionMode
{
    [InspectorName("Force Child Expand Height")]
    ForceChildExpandHeight,

    [InspectorName("Force Child Expand Width")]
    ForceChildExpandWidth,

    [InspectorName("Expand All")]
    ExpandAll,
}

public enum Constraint
{
    [InspectorName("Fixed Column Count")]
    FixedColumnCount,

    [InspectorName("Fixed Row Count")]
    FixedRowCount
}

public enum SizeMode
{
    Pixels,
    Percents
}

[ExecuteInEditMode]
[RequireComponent(typeof(LayoutGroup), typeof(RectTransform))]
public class CustomLayoutGroup : MonoBehaviour
{
    /// <summary>
    /// Choose if the grid has a fixed column number or a fixed row number.
    /// </summary>
    [SerializeField, Tooltip("Choose if the grid has a fixed column number or a fixed row number.")]
    private Constraint _constraint;

    /// <summary>
    /// Choose the fixed value of the constraint.
    /// </summary>
    [SerializeField, Tooltip("Choose the fixed value of the constraint.")]
    private int _constraintCount;

    /// <summary>
    /// Choose which aspect of a child will be forced to expand.
    /// </summary>
    [Space, SerializeField, Tooltip("Choose which aspect of a child will be forced to expand.")]
    private ExpansionMode _expansionMode;

    /// <summary>
    /// The ratio (in percents) that the aspect of a child which is not forced to expand must respect (Not used with 'Expand All').
    /// </summary>
    [SerializeField, Tooltip("The ratio (in percents) that the aspect of a child which is not forced to expand must respect (Not used with 'Expand All').")]
    private float _ratio = 50f;

    /// <summary>
    /// Choose if the padding is in percents or in pixels.
    /// </summary>
    [Space, SerializeField, Tooltip("Choose if the padding is in percents or in pixels.")]
    private SizeMode _paddingMode;

    /// <summary>
    /// Padding of the Layout Group.
    /// </summary>
    [SerializeField, Tooltip("Padding of the Layout Group.")]
    public RectOffset _padding = new();

    /// <summary>
    /// Choose if the spacing is in percents or in pixels.
    /// </summary>
    [Space, SerializeField, Tooltip("Choose if the spacing is in percents or in pixels.")]
    private SizeMode _spacingMode;

    /// <summary>
    /// Spacing between children.
    /// </summary>
    [SerializeField, Tooltip("Spacing between children.")]
    private float _spacing;

    /// <summary>
    /// Spacing between children.
    /// </summary>
    [SerializeField, Tooltip("Spacing between children.")]
    private Vector2 _gridSpacing;

    private LayoutGroup _layoutGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _layoutGroup = GetComponent<LayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateChildrenAspect();
    }

    private void UpdateChildrenAspect()
    {
        Vector2 rectSize = _rectTransform.rect.size;
        List<RectTransform> children = GetDirectChildren();

        switch (_paddingMode)
        {
            // Set paddings in pixels
            case SizeMode.Pixels:
                _layoutGroup.padding.left = _padding.left;
                _layoutGroup.padding.right = _padding.right;
                _layoutGroup.padding.top = _padding.top;
                _layoutGroup.padding.bottom = _padding.bottom;
                break;

            // Set paddings in percents
            case SizeMode.Percents:
                _layoutGroup.padding.left = Mathf.RoundToInt((_padding.left * rectSize.x) / 100f);
                _layoutGroup.padding.right = Mathf.RoundToInt((_padding.right * rectSize.x) / 100f);
                _layoutGroup.padding.top = Mathf.RoundToInt((_padding.top * rectSize.y) / 100f);
                _layoutGroup.padding.bottom = Mathf.RoundToInt((_padding.bottom * rectSize.y) / 100f);
                break;
        }

        switch (_layoutGroup)
        {
            // If the Layout Group is a Horizontal Layout Group
            case HorizontalLayoutGroup horizontalLayoutGroup:
                {
                    switch (_spacingMode)
                    {
                        // Set spacing in pixels
                        case SizeMode.Pixels:
                            horizontalLayoutGroup.spacing = _spacing;
                            break;

                        // Set spacing in percents
                        case SizeMode.Percents:
                            horizontalLayoutGroup.spacing = (_spacing * rectSize.x) / 100f;
                            break;
                    }

                    switch (_expansionMode)
                    {
                        case ExpansionMode.ForceChildExpandHeight:
                            {
                                // Calculate children height
                                float childrenHeight = rectSize.y - horizontalLayoutGroup.padding.vertical;

                                // Calculate children width
                                float childrenWidth = childrenHeight * (_ratio / 100f);

                                // Check if children width don't exceed the width of the parent
                                if ((childrenWidth * children.Count) + (horizontalLayoutGroup.spacing * (children.Count - 1)) + (horizontalLayoutGroup.padding.horizontal) > rectSize.x)
                                {
                                    // Calculate new children width
                                    childrenWidth = (rectSize.x / children.Count) - ((horizontalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (horizontalLayoutGroup.padding.horizontal / children.Count);

                                    // Calculate new children height
                                    childrenHeight = childrenWidth * (100f / _ratio);
                                }

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;

                        case ExpansionMode.ForceChildExpandWidth:
                            {
                                // Calculate children width
                                float childrenWidth = (rectSize.x / children.Count) - ((horizontalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (horizontalLayoutGroup.padding.horizontal / children.Count);

                                // Calculate children height
                                float childrenHeight = childrenWidth * (_ratio / 100f);

                                // Check if children height don't exceed the height of the parent
                                if (childrenHeight + horizontalLayoutGroup.padding.vertical > rectSize.y)
                                {
                                    // Calculate new children height
                                    childrenHeight = rectSize.y - horizontalLayoutGroup.padding.vertical;

                                    // Calculate new children width
                                    childrenWidth = childrenHeight * (100f / _ratio);
                                }

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;

                        case ExpansionMode.ExpandAll:
                            {
                                // Calculate children width
                                float childrenWidth = (rectSize.x / children.Count) - ((horizontalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (horizontalLayoutGroup.padding.horizontal / children.Count);

                                // Calculate children height
                                float childrenHeight = rectSize.y - horizontalLayoutGroup.padding.vertical;

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;
                    }
                }
                break;

            case VerticalLayoutGroup verticalLayoutGroup:
                {
                    switch (_spacingMode)
                    {
                        // Set spacing in pixels
                        case SizeMode.Pixels:
                            verticalLayoutGroup.spacing = _spacing;
                            break;

                        // Set spacing in percents
                        case SizeMode.Percents:
                            verticalLayoutGroup.spacing = (_spacing * rectSize.y) / 100f;
                            break;
                    }

                    switch (_expansionMode)
                    {
                        case ExpansionMode.ForceChildExpandHeight:
                            {
                                // Calculate children height
                                float childrenHeight = (rectSize.y / children.Count) - ((verticalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (verticalLayoutGroup.padding.vertical / children.Count);

                                // Calculate children width
                                float childrenWidth = childrenHeight * (_ratio / 100f);

                                // Check if children width don't exceed the width of the parent
                                if (childrenWidth + verticalLayoutGroup.padding.horizontal > rectSize.x)
                                {
                                    // Calculate new children width
                                    childrenWidth = rectSize.x - verticalLayoutGroup.padding.horizontal;

                                    // Calculate new children height
                                    childrenHeight = childrenWidth * (100f / _ratio);
                                }

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;

                        case ExpansionMode.ForceChildExpandWidth:
                            {
                                // Calculate children width
                                float childrenWidth = rectSize.x - verticalLayoutGroup.padding.horizontal;

                                // Calculate children height
                                float childrenHeight = childrenWidth * (_ratio / 100f);

                                // Check if children height don't exceed the height of the parent
                                if ((childrenHeight * children.Count) + (verticalLayoutGroup.spacing * (children.Count - 1)) + (verticalLayoutGroup.padding.vertical) > rectSize.x)
                                {
                                    // Calculate new children height
                                    childrenHeight = (rectSize.y / children.Count) - ((verticalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (verticalLayoutGroup.padding.vertical / children.Count);

                                    // Calculate new children width
                                    childrenWidth = childrenHeight * (100f / _ratio);
                                }

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;

                        case ExpansionMode.ExpandAll:
                            {
                                // Calculate children width
                                float childrenWidth = rectSize.x - verticalLayoutGroup.padding.horizontal;

                                // Calculate children height
                                float childrenHeight = (rectSize.y / children.Count) - ((verticalLayoutGroup.spacing * (children.Count - 1)) / children.Count) - (verticalLayoutGroup.padding.vertical / children.Count);

                                // Resize children
                                for (int i = 0; i < children.Count; i++)
                                {
                                    children[i].sizeDelta = new Vector2(childrenWidth, childrenHeight);
                                }
                            }
                            break;
                    }
                }
                break;

            case GridLayoutGroup gridLayoutGroup:
                {
                    switch (_spacingMode)
                    {
                        // Set spacing in pixels
                        case SizeMode.Pixels:
                            gridLayoutGroup.spacing = _gridSpacing;
                            break;

                        // Set spacing in percents
                        case SizeMode.Percents:
                            gridLayoutGroup.spacing = new Vector2(((_gridSpacing.x * rectSize.x) / 100f), ((_gridSpacing.y * rectSize.y) / 100f));
                            break;
                    }

                    int column = 1;
                    int row = 1;

                    switch (_constraint)
                    {
                        case Constraint.FixedColumnCount:
                            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                            gridLayoutGroup.constraintCount = _constraintCount;
                            column = gridLayoutGroup.constraintCount;
                            row = Mathf.CeilToInt(children.Count / (float)column);
                            break;
                        case Constraint.FixedRowCount:
                            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                            gridLayoutGroup.constraintCount = _constraintCount;
                            row = gridLayoutGroup.constraintCount;
                            column = Mathf.CeilToInt(children.Count / (float)row);
                            break;
                    }

                    switch (_expansionMode)
                    {
                        case ExpansionMode.ForceChildExpandHeight:
                            {
                                // Calculate cell height
                                float cellHeight = (rectSize.y / row) - ((gridLayoutGroup.spacing.y * (row - 1)) / row) - (gridLayoutGroup.padding.vertical / row);

                                // Calculate children width
                                float cellWidth = cellHeight * (_ratio / 100f);

                                // Check if cell width don't exceed the width of the parent
                                if ((cellWidth * column) + (gridLayoutGroup.spacing.x * (column - 1)) + (gridLayoutGroup.padding.horizontal) > rectSize.x)
                                {
                                    // Calculate new cell width
                                    cellWidth = (rectSize.x / column) - ((gridLayoutGroup.spacing.x * (column - 1)) / column) - (gridLayoutGroup.padding.horizontal / column);

                                    // Calculate new cell height
                                    cellHeight = cellWidth * (100f / _ratio);
                                }

                                // Resize cells
                                gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
                            }
                            break;

                        case ExpansionMode.ForceChildExpandWidth:
                            {
                                // Calculate cell width
                                float cellWidth = (rectSize.x / column) - ((gridLayoutGroup.spacing.x * (column - 1)) / column) - (gridLayoutGroup.padding.horizontal / column);

                                // Calculate cell height
                                float cellHeight = cellWidth * (_ratio / 100f);

                                // Check if cell height don't exceed the height of the parent
                                if ((cellHeight * row) + (gridLayoutGroup.spacing.y * (row - 1)) + (gridLayoutGroup.padding.vertical) > rectSize.y)
                                {
                                    // Calculate new cell height
                                    cellHeight = (rectSize.y / row) - ((gridLayoutGroup.spacing.y * (row - 1)) / row) - (gridLayoutGroup.padding.vertical / row);

                                    // Calculate new cell width
                                    cellWidth = cellHeight * (100f / _ratio);
                                }

                                // Resize cells
                                gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
                            }
                            break;

                        case ExpansionMode.ExpandAll:
                            {
                                // Calculate cell width
                                float cellWidth = (rectSize.x / column) - ((gridLayoutGroup.spacing.x * (column - 1)) / column) - (gridLayoutGroup.padding.horizontal / column);

                                // Calculate cell height
                                float cellHeight = (rectSize.y / row) - ((gridLayoutGroup.spacing.y * (row - 1)) / row) - (gridLayoutGroup.padding.vertical / row);

                                // Resize cells
                                gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
                            }
                            break;
                    }
                }
                break;
        }

        // Force update of the layout group
        LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Called to get the list of activated direct children.
    /// </summary>
    /// <returns></returns>
    private List<RectTransform> GetDirectChildren()
    {
        List<RectTransform> children = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                children.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
        }

        return children;
    }
}