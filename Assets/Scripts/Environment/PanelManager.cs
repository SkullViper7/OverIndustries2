using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    Dictionary<string, Dictionary<string, GameObject>> _grid = new();
    const string _rowFormat = "row{0}";
    const string _columnFormat = "column{0}";

    void Start()
    {
        GridManager.Instance.GridInitializedEvent += InitPanel;
    }

    public void InitPanel()
    {
        // Create all slots in the dictionnary
        for (int i = 0; i < GridManager.Instance.GridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, GameObject>());

            for (int j = 0; j < GridManager.Instance.GridSize.x; j++)
            {
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
                InstantiatePanel(new Vector2(j, i));    
            }
        }

        for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
        {
            Room room = GridManager.Instance.InstantiatedRooms[i];

            for (int j = 0; j < room.RoomData.Size; j++)
            {
                DeactivatePanel(room.RoomPosition + new Vector2(j, 0));
            }
        }

        GridManager.Instance.RoomRemoveEvent += DeactivatePanel;
        GridManager.Instance.RoomAddEvent += InstantiatePanel;
    }

    public void InstantiatePanel(Vector2 position)
    {
        GameObject panel = Instantiate(_panel, GridManager.Instance.ConvertGridPosIntoWorldPos(position), Quaternion.identity, transform);

        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)] = panel;
    }

    public void ActivatePanel(Vector2 position)
    {
        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)].SetActive(true);
    }

    public void DeactivatePanel(Vector2 position)
    {
        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)].SetActive(false);
    }
}
