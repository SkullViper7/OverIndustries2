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

    /// <summary>
    /// Called to initialize the panel grid.
    /// </summary>
    public void InitPanel()
    {
        // Create all slots in the dictionnary
        for (int i = 0; i < GridManager.Instance.GridSize.y; i++)
        {
            _grid.Add(string.Format(_rowFormat, i), new Dictionary<string, GameObject>());

            for (int j = 0; j < GridManager.Instance.GridSize.x; j++)
            {
                // Create a new panel at the specified position
                _grid[string.Format(_rowFormat, i)].Add(string.Format(_columnFormat, j), null);
                InstantiatePanel(new Vector2(j, i));    
            }
        }

        // Disable all panels that are inside a room
        for (int i = 0; i < GridManager.Instance.InstantiatedRooms.Count; i++)
        {
            Room room = GridManager.Instance.InstantiatedRooms[i];

            for (int j = 0; j < room.RoomData.Size; j++)
            {
                // Deactivate the panel at the specified position
                DeactivatePanel(room.RoomPosition + new Vector2(j, 0));
            }
        }

        // Set up event listeners to react to room events
        GridManager.Instance.RoomRemoveEvent += DeactivatePanel;
        GridManager.Instance.RoomAddEvent += InstantiatePanel;
    }

    /// <summary>
    /// Instantiates a new panel at the specified position in the grid.
    /// </summary>
    /// <param name="position">Position in the grid.</param>
    public void InstantiatePanel(Vector2 position)
    {
        // Instantiate a new panel at the world position corresponding to the grid position
        GameObject panel = Instantiate(_panel, GridManager.Instance.ConvertGridPosIntoWorldPos(position), Quaternion.identity, transform);

        // Store the panel in the grid
        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)] = panel;
    }

    /// <summary>
    /// Activates a panel at the specified position in the grid.
    /// </summary>
    /// <param name="position">Position in the grid.</param>
    public void ActivatePanel(Vector2 position)
    {
        // Activate the panel at the specified position
        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)].SetActive(true);
    }

    /// <summary>
    /// Deactivates a panel at the specified position in the grid.
    /// </summary>
    /// <param name="position">Position in the grid.</param>
    public void DeactivatePanel(Vector2 position)
    {
        // Deactivate the panel at the specified position
        _grid[string.Format(_rowFormat, position.y)][string.Format(_columnFormat, position.x)].SetActive(false);
    }
}
