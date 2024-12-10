using UnityEngine;
using UnityEngine.UI;

public class GridLayoutGroupFitter : MonoBehaviour
{
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _rectTransform;

    [SerializeField]
    private int _heightRatio;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Debug.Log("test");

        int raw = _gridLayoutGroup.constraintCount;

        float cellsizeY = _rectTransform.sizeDelta.y / (_heightRatio * raw) * _heightRatio;

        _gridLayoutGroup.cellSize = new Vector2(cellsizeY, cellsizeY);
    }
}
